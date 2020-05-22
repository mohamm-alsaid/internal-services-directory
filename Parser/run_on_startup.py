from urllib.error import HTTPError
from urllib.error import URLError
import PDF_parser
import read_into_database
import sys
import urllib
from urllib import request
import PyPDF2
import time;
import os;

# takes in the args from the command line
# exits if there are greater than or less than 3 args, sys.argv[0] is always the program name

pdf_links = []
print('Enter pdf links below, enter q or Q when you\'re done...')

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
if len(pdf_links) > 0:
    for pdf_link in pdf_links:
        try:
            output_files.append('output' + str(pdf_num) + '.json')
            urllib.request.urlretrieve(pdf_link, "input" + str(pdf_num) + ".pdf")
            # checks for valid PDF file
            a = open("input" + str(pdf_num) + ".pdf", "rb")
            PyPDF2.PdfFileReader(a)
        except PyPDF2.utils.PdfReadError:
            # this line, os.remove(filename), might be redundant since we're just going to overwrite it anyway on next
            # program execution
            a.close()
            os.remove("input" + str(pdf_num) + ".pdf")
            print("Invalid PDF file downloaded from, " + pdf_link)
            os.system('pause')
            sys.exit(-2)
        except ValueError:
            print("Invalid url, " + pdf_link)
            os.system('pause')
            sys.exit(-3)
        except (HTTPError, URLError):
            print("HTTP Error 404: Given Url, " + pdf_link + ", not found")
            os.system('pause')
            sys.exit(-4)
        else:
            a.close()
            pass
        pdf_files.append("input" + str(pdf_num) + ".pdf")
        pdf_num += 1

    for pdf, json in zip(pdf_files, output_files):
        PDF_parser.parse(pdf, json)

    read_into_database.read_to_database(output_files)
else:
    print("No links entered")
    os.system('pause')
    sys.exit(-6)
