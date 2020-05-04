using System;
using System.Collections.Generic;
using System.Text;
using MultCo_ISD_API.Models;
using NUnit.Framework;
using FluentValidation;
using FluentValidation.TestHelper;
using MultCo_ISD_API.V1.Validators;

namespace ValidatorUnitTest
{
    class ValidatorsUnitTest
    {
        private ContactValidator ContactVal;
        private CommunityValidator CommunityVal;
        private DivisionValidator DivisionVal;
        private LanguageValidator LanguageVal;
        private LocationTypeValidator LocationTypeVal;
        private LocationValidator LocationVal;
        private DepartmentValidator DepartmentVal;
        private ServiceValidator ServiceVal;
        private ProgramValidator ProgramVal;
        private ServiceLanguageAssociationValidator ServiceLanguageVal;
        private ServiceLocationAssociationValidator ServiceLocationVal;
        private ServiceCommunityAssociationValidator ServiceCommunityVal;

        [SetUp]
        public void SetUp()
        {
            ContactVal = new ContactValidator();
            CommunityVal = new CommunityValidator();
            DivisionVal = new DivisionValidator();
            LanguageVal = new LanguageValidator();
            LocationTypeVal = new LocationTypeValidator();
            LocationVal = new LocationValidator();
            DepartmentVal = new DepartmentValidator();
            ServiceVal = new ServiceValidator();
            ProgramVal = new ProgramValidator();
            ServiceLanguageVal = new ServiceLanguageAssociationValidator();
            ServiceLocationVal = new ServiceLocationAssociationValidator();
            ServiceCommunityVal = new ServiceCommunityAssociationValidator();
        }
        [Test]
        public void should_not_have_errors()
        {
            //TODO: create a unit test to check if the validators have been run during a post request (maybe in controllers unit test)
            //TODO: create a unit test to check the return message from creating a post request with valid input (maybe in controllers unit test)
            //TODO: create a unit test to check the return message from creating a post request with invalid input (maybe in controller unit test)


        }
        //Department
        [Test]
        public void Department_should_not_have_errors()
        {

            // -- SUCCESS department init..
            var dept = new Department();
            dept.DepartmentCode = 1;
            dept.DepartmentName = "some name";
            var deptResult = DepartmentVal.TestValidate(dept);

            // assertions -- department
            deptResult.ShouldNotHaveValidationErrorFor(x => x.DepartmentName);
            deptResult.ShouldNotHaveValidationErrorFor(x => x.DepartmentCode);
        }
        [Test]
        public void Department_should_have_errors()
        {
            var validator = new DepartmentValidator();
            validator.ShouldHaveValidationErrorFor(x => x.DepartmentName, null as string);
            validator.ShouldHaveValidationErrorFor(x => x.DepartmentName,  new string('x',21));
        }
        //contact
        [Test]
        public void Contact_should_not_have_errors()
        {
            var contact = new Contact();
            contact.ContactName = "a valid name";
            contact.PhoneNumber = "555-555-5555";
            contact.EmailAddress = "vali@mult.co";
            var Result = ContactVal.TestValidate(contact);

            // assertions...
            Result.ShouldNotHaveValidationErrorFor(x => x.ContactName);
            Result.ShouldNotHaveValidationErrorFor(x => x.PhoneNumber);
            Result.ShouldNotHaveValidationErrorFor(x => x.EmailAddress);
        }
        [Test]
        public void Contact_should_have_errors()
        {
            ContactVal.ShouldHaveValidationErrorFor(x => x.ContactName, null as string);
            ContactVal.ShouldHaveValidationErrorFor(x => x.ContactName, new string('x', 21));

            ContactVal.ShouldHaveValidationErrorFor(x => x.PhoneNumber, null as string);
            // tests two cases
            ContactVal.ShouldHaveValidationErrorFor(x => x.PhoneNumber, new string('1',14));

            ContactVal.ShouldHaveValidationErrorFor(x => x.EmailAddress, null as string);
            ContactVal.ShouldHaveValidationErrorFor(x => x.EmailAddress, new string('a', 21));


        }

        //Program
        [Test]
        public void Program_should_not_have_errors()
        {
            var program = new Program();
            program.SponsorName = "a valid name";
            program.OfferType = "1";
            program.ProgramName = "a valid name";
            program.ProgramOfferNumber = "1";
            var Result = ProgramVal.TestValidate(program);

            // assertions...
            Result.ShouldNotHaveValidationErrorFor(x => x.SponsorName);
            Result.ShouldNotHaveValidationErrorFor(x => x.OfferType);
            Result.ShouldNotHaveValidationErrorFor(x => x.ProgramName);
            Result.ShouldNotHaveValidationErrorFor(x => x.ProgramOfferNumber);

        }
        [Test]
        public void Program_should_have_errors()
        {
            ProgramVal.ShouldHaveValidationErrorFor(x => x.SponsorName, null as string);
            ProgramVal.ShouldHaveValidationErrorFor(x => x.SponsorName, new string('x', 21));

            ProgramVal.ShouldHaveValidationErrorFor(x => x.ProgramName, null as string);
            ProgramVal.ShouldHaveValidationErrorFor(x => x.ProgramName, new string('x', 21));

            ProgramVal.ShouldHaveValidationErrorFor(x => x.OfferType, null as string);
            ProgramVal.ShouldHaveValidationErrorFor(x => x.OfferType, new string('x', 21));

            ProgramVal.ShouldHaveValidationErrorFor(x => x.ProgramOfferNumber, null as string);
            ProgramVal.ShouldHaveValidationErrorFor(x => x.ProgramOfferNumber, new string('x', 6));


        }
        // LocationType
        [Test]
        public void Location_should_not_have_errors()
        {
            var location= new Location();
            location.LocationTypeId = 1;
            location.LocationName = "a valid name";
            location.BuildingId= "1";
            location.LocationAddress = "506 SW Mill St, Portland, OR 97201";
            location.RoomNumber = "1";
            location.FloorNumber = "1";
            var Result = LocationVal.TestValidate(location);

            // assertions...
            Result.ShouldNotHaveValidationErrorFor(x => x.LocationTypeId);
            Result.ShouldNotHaveValidationErrorFor(x => x.LocationName);
            Result.ShouldNotHaveValidationErrorFor(x => x.BuildingId);
            Result.ShouldNotHaveValidationErrorFor(x => x.LocationAddress);
            Result.ShouldNotHaveValidationErrorFor(x => x.RoomNumber);
            Result.ShouldNotHaveValidationErrorFor(x => x.FloorNumber);

        }
        [Test]
        public void Location_should_have_errors()
        {
            LocationVal.ShouldHaveValidationErrorFor(x => x.LocationName, null as string);
            LocationVal.ShouldHaveValidationErrorFor(x => x.LocationName, new string('x', 21));

            LocationVal.ShouldHaveValidationErrorFor(x => x.BuildingId, null as string);

            LocationVal.ShouldHaveValidationErrorFor(x => x.LocationAddress, null as string);
            LocationVal.ShouldHaveValidationErrorFor(x => x.LocationAddress, new string('x', 51));

            LocationVal.ShouldHaveValidationErrorFor(x => x.RoomNumber, null as string);
            LocationVal.ShouldHaveValidationErrorFor(x => x.FloorNumber, null as string);
        }

        // Location
        [Test]
        public void LocationType_should_not_have_errors()
        {
            var locationType = new LocationType();
            locationType.LocationTypeName = "valid name";

            var Result = LocationTypeVal.TestValidate(locationType);

            // assertions...
            Result.ShouldNotHaveValidationErrorFor(x => x.LocationTypeName);
        }
        [Test]
        public void LocationType_should_have_errors()
        {
            LocationTypeVal.ShouldHaveValidationErrorFor(x => x.LocationTypeName, null as string);
            LocationTypeVal.ShouldHaveValidationErrorFor(x => x.LocationTypeName, new string('x', 21));
        }

        //division
        [Test]
        public void Division_should_not_have_errors()
        {
            var division = new Division();
            division.DivisionName= "valid name";

            division.DivisionCode = 1;

            var Result = DivisionVal.TestValidate(division);

            // assertions...
            Result.ShouldNotHaveValidationErrorFor(x => x.DivisionCode);
            Result.ShouldNotHaveValidationErrorFor(x => x.DivisionName);
        }
        [Test]
        public void Division_should_have_errors()
        {
            DivisionVal.ShouldHaveValidationErrorFor(x => x.DivisionName, null as string);
            DivisionVal.ShouldHaveValidationErrorFor(x => x.DivisionName, new string('x', 21));
        }
        //language
        [Test]
        public void Language_should_not_have_errors()
        {
            var language = new Language();
            language.LanguageName = "valid name";

            var Result = LanguageVal.TestValidate(language);

            // assertions...
            Result.ShouldNotHaveValidationErrorFor(x => x.LanguageName);
        }
        [Test]
        public void Language_should_have_errors()
        {
            LanguageVal.ShouldHaveValidationErrorFor(x => x.LanguageName, null as string);
            LanguageVal.ShouldHaveValidationErrorFor(x => x.LanguageName, new string('x', 21));
        }
        // community
        [Test]
        public void Community_should_not_have_errors()
        {

            // -- Test init..
            var community = new Community();
            community.CommunityName = "valid name";
            community.CommunityDescription= "A valid description";
            

            var Result = CommunityVal.TestValidate(community);

            // assertions..
            Result.ShouldNotHaveValidationErrorFor(x => x.CommunityName);
            Result.ShouldNotHaveValidationErrorFor(x => x.CommunityDescription);
        }
        [Test]
        public void Community_should_have_errors()
        {
            CommunityVal.ShouldHaveValidationErrorFor(x => x.CommunityId, 0);
            CommunityVal.ShouldHaveValidationErrorFor(x => x.CommunityId, -1);

            CommunityVal.ShouldHaveValidationErrorFor(x => x.CommunityName, null as string);
            CommunityVal.ShouldHaveValidationErrorFor(x => x.CommunityName, new string('x', 51));

            CommunityVal.ShouldHaveValidationErrorFor(x => x.CommunityDescription, null as string);
            CommunityVal.ShouldHaveValidationErrorFor(x => x.CommunityDescription, new string('x', 51));
        }
        // ServiceCommunityAssociation
        [Test]
        public void ServiceCommunity_should_not_have_errors()
        {
            // -- Test init..
            var ServiceCommunity = new ServiceCommunityAssociation();
            ServiceCommunity.ServiceId = 5;
            ServiceCommunity.CommunityId = 2;

            var Result = ServiceCommunityVal.TestValidate(ServiceCommunity);

            // assertions..
            Result.ShouldNotHaveValidationErrorFor(x => x.ServiceId);
            Result.ShouldNotHaveValidationErrorFor(x => x.CommunityId);
        }
        [Test]
        public void ServiceCommunity_should_have_errors()
        {
            ServiceCommunityVal.ShouldHaveValidationErrorFor(x => x.ServiceCommunityAssociationId, 0);
            ServiceCommunityVal.ShouldHaveValidationErrorFor(x => x.ServiceCommunityAssociationId, -1);


            ServiceCommunityVal.ShouldHaveValidationErrorFor(x => x.CommunityId, 0);
            ServiceCommunityVal.ShouldHaveValidationErrorFor(x => x.CommunityId, -1);


            ServiceCommunityVal.ShouldHaveValidationErrorFor(x => x.ServiceId, 0);
            ServiceCommunityVal.ShouldHaveValidationErrorFor(x => x.ServiceId, -1);

        }
        /**///ServiceLanguageAssociation
        [Test]
        public void ServiceLanguage_should_not_have_errors()
        {
            // -- Test init..
            var ServiceLanguage = new ServiceLanguageAssociation();
            ServiceLanguage.ServiceId = 2;
            ServiceLanguage.LanguageId = 5;

            var Result = ServiceLanguageVal.TestValidate(ServiceLanguage);

            // assertions..
            Result.ShouldNotHaveValidationErrorFor(x => x.ServiceId);
            Result.ShouldNotHaveValidationErrorFor(x => x.LanguageId);
        }
        [Test]
        public void ServiceLanguage_should_have_errors()
        {
            ServiceLanguageVal.ShouldHaveValidationErrorFor(x => x.ServiceLanguageAssociation1, 0);
            ServiceLanguageVal.ShouldHaveValidationErrorFor(x => x.ServiceLanguageAssociation1, -1);


            ServiceLanguageVal.ShouldHaveValidationErrorFor(x => x.LanguageId, 0);
            ServiceLanguageVal.ShouldHaveValidationErrorFor(x => x.LanguageId, -1);


            ServiceLanguageVal.ShouldHaveValidationErrorFor(x => x.ServiceId, 0);
            ServiceLanguageVal.ShouldHaveValidationErrorFor(x => x.ServiceId, -1);
        }
        [Test]
        public void ServiceLocation_should_not_have_errors()
        {
            // -- Test init..
            var ServiceLocation = new ServiceLocationAssociation();
            ServiceLocation.ServiceId = 1;
            ServiceLocation.LocationId = 2;

            var Result = ServiceLocationVal.TestValidate(ServiceLocation);

            // assertions..
            Result.ShouldNotHaveValidationErrorFor(x => x.ServiceId);
            Result.ShouldNotHaveValidationErrorFor(x => x.LocationId);
        }
        [Test]
        public void ServiceLocation_should_have_errors()
        {
            ServiceLocationVal.ShouldHaveValidationErrorFor(x => x.ServiceLocationAssociation1, 0);
            ServiceLocationVal.ShouldHaveValidationErrorFor(x => x.ServiceLocationAssociation1, -1);

            ServiceLocationVal.ShouldHaveValidationErrorFor(x => x.LocationId, 0);
            ServiceLocationVal.ShouldHaveValidationErrorFor(x => x.LocationId, -1);

            ServiceLocationVal.ShouldHaveValidationErrorFor(x => x.ServiceId, 0);
            ServiceLocationVal.ShouldHaveValidationErrorFor(x => x.ServiceId, -1);
        }

        // Service
        [Test]
        public void Service_should_not_have_errors()
        {
            ServiceVal.ShouldNotHaveValidationErrorFor(x => x.ServiceId, 1);
            ServiceVal.ShouldNotHaveValidationErrorFor(x => x.ProgramId, 1);
            ServiceVal.ShouldNotHaveValidationErrorFor(x => x.DivisionId, 1);
            ServiceVal.ShouldNotHaveValidationErrorFor(x => x.ContactId, 1);
            ServiceVal.ShouldNotHaveValidationErrorFor(x => x.ServiceName, "valid name");
            ServiceVal.ShouldNotHaveValidationErrorFor(x => x.ServiceDescription, "valid description");
            ServiceVal.ShouldNotHaveValidationErrorFor(x => x.ExecutiveSummary, "valid executive summary");
            ServiceVal.ShouldNotHaveValidationErrorFor(x => x.ServiceArea, "valid service area");


            ServiceVal.ShouldNotHaveValidationErrorFor(x => x.EmployeeConnectMethod, "valid Employee Connect Method");
            ServiceVal.ShouldNotHaveValidationErrorFor(x => x.CustomerConnectMethod, "valid Customer Connect Method");
            
            ServiceVal.ShouldNotHaveValidationErrorFor(x => x.ExpirationDate, default(DateTime));
            ServiceVal.ShouldNotHaveValidationErrorFor(x => x.Active, true);

            ServiceVal.ShouldNotHaveValidationErrorFor(x => x.Contact, new Contact());
            ServiceVal.ShouldHaveChildValidator(x => x.Contact, typeof(ContactValidator));

            ServiceVal.ShouldNotHaveValidationErrorFor(x => x.Program, new Program());
            ServiceVal.ShouldHaveChildValidator(x => x.Program, typeof(ProgramValidator));
         
            ServiceVal.ShouldNotHaveValidationErrorFor(x => x.Department, new Department());
            ServiceVal.ShouldHaveChildValidator(x => x.Department, typeof(DepartmentValidator));


            ServiceVal.ShouldNotHaveValidationErrorFor(x => x.Division, new Division());
            ServiceVal.ShouldHaveChildValidator(x => x.Division, typeof(DivisionValidator));

        }
        [Test]
        public void Service_should_have_errors()
        {
            ServiceVal.ShouldHaveValidationErrorFor(x => x.ServiceId, 0);
            ServiceVal.ShouldHaveValidationErrorFor(x => x.ServiceId, -1);

            ServiceVal.ShouldHaveValidationErrorFor(x => x.ProgramId, 0);
            ServiceVal.ShouldHaveValidationErrorFor(x => x.ProgramId, -1);

            ServiceVal.ShouldHaveValidationErrorFor(x => x.DivisionId, 0);
            ServiceVal.ShouldHaveValidationErrorFor(x => x.DivisionId, -1);

            ServiceVal.ShouldHaveValidationErrorFor(x => x.ContactId, 0);
            ServiceVal.ShouldHaveValidationErrorFor(x => x.ContactId, -1);

            ServiceVal.ShouldHaveValidationErrorFor(x => x.ServiceName, null as string);
            ServiceVal.ShouldHaveValidationErrorFor(x => x.ServiceName, new string('x',21));
           
            ServiceVal.ShouldHaveValidationErrorFor(x => x.ServiceDescription, null as string);
            ServiceVal.ShouldHaveValidationErrorFor(x => x.ServiceDescription, new string('x', 51));
          
            ServiceVal.ShouldHaveValidationErrorFor(x => x.ExecutiveSummary, null as string);
            ServiceVal.ShouldHaveValidationErrorFor(x => x.ExecutiveSummary, new string('x', 51));

            ServiceVal.ShouldHaveValidationErrorFor(x => x.ServiceArea, null as string);
            ServiceVal.ShouldHaveValidationErrorFor(x => x.ServiceArea, new string('x', 21));

            ServiceVal.ShouldHaveValidationErrorFor(x => x.EmployeeConnectMethod, null as string);
            ServiceVal.ShouldHaveValidationErrorFor(x => x.EmployeeConnectMethod, new string('x', 51));

            ServiceVal.ShouldHaveValidationErrorFor(x => x.CustomerConnectMethod, null as string);
            ServiceVal.ShouldHaveValidationErrorFor(x => x.CustomerConnectMethod, new string('x', 51));

            DateTime? date = null;
            ServiceVal.ShouldHaveValidationErrorFor(x => x.ExpirationDate, date);

        }
    }
}
