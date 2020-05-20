import PDF_parser
import read_into_database
import sys
import urllib
from urllib import request
import PyPDF2

# takes in the args from the command line
# exits if there are greater than or less than 3 args, sys.argv[0] is always the program name

pdf_links = []
print('Enter pdf links below, enter q or Q to exit...')

while True:
    buffer = input()
    if buffer == "q" or buffer == "Q":
        break;
    else:
        pdf_links.append(buffer)

pdf_files = []
output_files = []
pdf_num = 0

# downloads the pdf file at the location
for pdf_link in pdf_links:
    output_files.append('output' + str(pdf_num) + '.json')
    urllib.request.urlretrieve(pdf_link, "input" + str(pdf_num) + ".pdf")
    # checks for valid PDF file
    try:
        a = open("input" + str(pdf_num) + ".pdf", "rb")
        PyPDF2.PdfFileReader(a)
    except PyPDF2.utils.PdfReadError:
        # this line, os.remove(filename), might be redundant since we're just going to overwrite it anyway on next
        # program execution
        # os.remove(filename)
        print("Invalid PDF file downloaded")
        sys.exit(-2)
    else:
        a.close()
        pass
    pdf_files.append("input" + str(pdf_num) + ".pdf")
    pdf_num += 1

for pdf, json in zip(pdf_files, output_files):
    PDF_parser.parse(pdf, json)

read_into_database.read_to_database(output_files)
