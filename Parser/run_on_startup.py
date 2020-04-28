import PDF_parser
import read_into_database
import sys
import urllib
from urllib import request
import PyPDF2

# takes in the args from the command line
# exits if there are greater than or less than 3 args, sys.argv[0] is always the program name
if len(sys.argv) != 3 and len(sys.argv[1]) != len(sys.argv[2]):
    print("Usage: <" + sys.argv[0] + "> <[PDF url 1, PDF url 2, ...]> <[output json filename 1, output json filename "
                                     "2, ...]>")
    sys.exit(-1)

pdf_files = []
pdf_num = 0

# downloads the pdf file at the location
for pdf_file in sys.argv[1]:
    urllib.request.urlretrieve(pdf_file, "input" + pdf_num + ".pdf")
    # checks for valid PDF file
    try:
        a = open(pdf_file, "rb")
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
    pdf_files.append("input" + pdf_num + ".pdf")
    pdf_num += 1


output_filename = sys.argv[2]
