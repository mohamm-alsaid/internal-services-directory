from io import StringIO
from pdfminer.converter import TextConverter
from pdfminer.layout import LAParams
from pdfminer.pdfinterp import PDFResourceManager, PDFPageInterpreter
from pdfminer.pdfpage import PDFPage
import json
import re
from tqdm import tqdm

# https://multco.us/file/82166/download
# https://multco.us/file/82174/download

keys = {
    "Department",
    "Program Contact",
    "Program Offer Type",
    "Program Offer Stage",
    "Related Programs",
    "Program Characteristics",
    "Executive Summary",
    "Program Summary"
}
departments = {
    "Community Justice",
    "Community Services",
    "County Assets",
    "County Management",
    "District Attorney",
    "Sheriff",
    "County Human Services",
    "Health Department",
    "Library",
    "Nondepartmental"
}
program_offer_types = {
    "Existing Operating Program",
    "Support",
    "Administration",
    "Innovative/New Program",
    "Internal Service"
}
program_characteristics = {
    "One-Time-Only Request",
    "Backfill State/Federal/Grant",
    "Measure 5 Education"
}

program_contacts = {}
words = set()


def parse(pdf_file, output_file):
    pages = extract_text(pdf_file)
    lines = {"Executive Summary", "Program Summary", "Performance Measures Descriptions"}
    lines.update(keys)
    lines.update(departments)
    lines.update(program_offer_types)
    lines.update(program_characteristics)
    for line in lines:
        word = line.split(' ')
        for w in word:
            words.add(w)

    pages = [p for p in pages if p.startswith("Program #")]
    data = []
    for p in pages:
        d = read_page(p)
        data.append(d)
    with open(output_file, "w") as f:
        json.dump(data, f, indent="\t")

    with open(output_file, 'r') as f:
        json_str = f.read()
        data = json.loads(json_str)
        for item in data:
            result = re.findall(r'(\d+)', item["Program Contact"])
            error_name = item["Program Name"]
            if result:
                print(result)
                print("errors are in :", error_name, '\n')


def extract_text(pdf_file, password='', page_numbers=None, maxpages=0,
                 caching=True, codec='utf-8', laparams=None):
    if laparams is None:
        laparams = LAParams()
    prev = ""
    pages = []
    with open(pdf_file, "rb") as fp:
        output_string = StringIO()
        rsrcmgr = PDFResourceManager()
        device = TextConverter(rsrcmgr, output_string,
                               laparams=laparams)
        interpreter = PDFPageInterpreter(rsrcmgr, device)
        pro = 0
        for page in tqdm(PDFPage.get_pages(
                fp, page_numbers, maxpages=maxpages,
                password=password, caching=caching,
                check_extractable=True,
        )):
            interpreter.process_page(page)
            current = output_string.getvalue()
            pages.append(current[len(prev):])
            prev = current
            pro += 1
        print("text extracted")
        return pages


def read_page(page):
    page = page.split("\n")
    data = {k: "" for k in keys}
    temp = re.split(r'\W', page[0])
    temp2 = []
    for x in temp:
        if x != '':
            temp2.append(x)
    data["Program Number"] = temp2[1]
    data["Program Name"] = (' '.join(temp2[2:])).rstrip()
    print(data["Program Name"])
    data["Executive Summary"] = ""
    data["Program Summary"] = ""
    data["Related Programs"] = ""

    need_contact = True

    start_exec = False
    finish_exec = False
    start_prog = False
    finish_prog = False

    for i, line in enumerate(page):
        line = line.encode('ascii', 'ignore')
        line = line.decode('ascii', 'ignore')
        if line == '':
            continue
        elif line[-1] == ':' or i == 0 or line.startswith("www"):
            continue
        if line in departments:
            data["Department"] = line

        elif line in program_offer_types:
            data["Program Offer Type"] = line

        elif line in program_contacts:
            data["Program Contacts"] = line

        elif line.startswith("Program Characteristics:"):
            data["Program Characteristics"] = line.split(": ")[1]

        elif line.startswith("Program Offer Stage: "):
            data["Program Offer Stage"] = line.split(": ")[-1]

        elif i < 12 and line[0].isdigit() and line[4].isdigit() and "/" not in line:
            nums = line.split(" ")
            for num in nums:
                num = num.strip()
                num = num.strip(',')
                num = num.strip(';')
                data["Related Programs"] += (num + " ")
            data["Related Programs"] = data["Related Programs"].strip()

        elif (line.count(' ') == 1 or line.count(' ') == 2) and need_contact:
            potential_name = line.split(' ')
            is_name = True
            for word in potential_name:
                if word in words:
                    is_name = False
            if is_name:
                data["Program Contact"] = line
                need_contact = False

        # we're at the start of one of the summaries
        elif len(line) > 40:
            # if we're at the beginning of executive summary
            if not start_exec:
                start_exec = True
            # if we're at the beginning of program summary
            elif finish_exec and not start_prog:
                start_prog = True

        if start_exec and not finish_exec:
            if line == "Program Summary":
                finish_exec = True
            else:
                data["Executive Summary"] += line

        if start_prog and not finish_prog:
            if line == "Performance Measures":
                return data
            else:
                data["Program Summary"] += line

    return data

