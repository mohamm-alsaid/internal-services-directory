import pyodbc
import json


def read_to_database(file_names):
    department_codes = {
        'Nondepartmental': 10,
        'District Attorney': 15,
        'County Human Services': 20,
        'Health Department': 40,
        'Community Justice': 50,
        'Sheriff': 60,
        'County Management': 72,
        'County Assets': 78,
        'Library': 80,
        'Community Services': 91,
        'Overall County': 95
    }

    # Set up temp dictionaries for easier FK relation searching
    department_inc = 1
    departments = {}

    contact_inc = 1
    contacts = {}

    program_inc = 1
    programs = {}

    server = 'localhost'
    database = 'InternalServicesDirectoryV1'
    username = 'db0'

    conn = pyodbc.connect('DRIVER={ODBC Driver 17 for SQL Server};'
                          'SERVER=' + server + ';'
                                               'DATABASE=' + database + ';'
                                                                        'USERNAME=' + username + ';'
                                                                                                 'Trusted_Connection'
                                                                                                 '=yes;')
    cursor = conn.cursor()

    for file_name in file_names:
        # Read json into a str
        file = open(file_name, 'r')
        a = file.read()

        # Convert str into dictionary
        x = json.loads(a)

        for item in x:
            if item['Department'] not in departments and item['Department'] != "":
                departments[item['Department']] = department_inc
                cursor.execute("""INSERT INTO Department VALUES (?, ?)""", department_codes.get(item['Department']),
                               item['Department'])
                department_inc += 1
                conn.commit()
            if item['Program Contact'] not in contacts and item['Program Contact'] != "":
                contacts[item['Program Contact']] = contact_inc
                cursor.execute("""INSERT INTO Contact (contactName) VALUES (?)""", item['Program Contact'])
                contact_inc += 1
                conn.commit()
            if item['Program Number'] not in programs and item['Program Number'] != "":
                programs[item['Program Number']] = program_inc
                offer_type = item['Program Offer Type']
                if offer_type == "":
                    offer_type = None
                cursor.execute("""INSERT INTO Program (offerType, programName, programOfferNumber) VALUES (?, ?, ?)""",
                               offer_type, item['Program Name'], item['Program Number'])
                program_inc += 1
                conn.commit()
            cursor.execute("""INSERT INTO Service (programID, departmentID, serviceName, serviceDescription, 
                executiveSummary, contactID, active) VALUES (?, ?, ?, ?, ?, ?, ?)""", programs.get(item['Program Number']),
                           departments.get(
                               item['Department']), item['Program Name'], item['Program Summary'],
                           item['Executive Summary'], contacts.get(
                    item['Program Contact']), True)
            conn.commit()

    conn.commit()
    cursor.close()
    conn.close()
