using System;
using System.Collections.Generic;
using System.Text;
using MultCo_ISD_API.V1.DTO;
using NUnit.Framework;
using FluentValidation;
using FluentValidation.TestHelper;
using MultCo_ISD_API.V1.Validators;

namespace ValidatorUnitTest
{
    class ValidatorsUnitTest
    {
        private ContactV1DTOValidator ContactVal;
        private CommunityV1DTOValidator CommunityVal;
        private DivisionV1DTOValidator DivisionVal;
        private LanguageV1DTOValidator LanguageVal;
        private LocationTypeV1DTOValidator LocationTypeVal;
        private LocationV1DTOValidator LocationVal;
        private DepartmentV1DTOValidator DepartmentVal;
        private ServiceV1DTOValidator ServiceVal;
        private ProgramV1DTOValidator ProgramVal;
        private ServiceLanguageAssociationV1DTOValidator ServiceLanguageVal;
        private ServiceLocationAssociationV1DTOValidator ServiceLocationVal;
        private ServiceCommunityAssociationV1DTOValidator ServiceCommunityVal;

        [SetUp]
        public void SetUp()
        {
            ContactVal = new ContactV1DTOValidator();
            CommunityVal = new CommunityV1DTOValidator();
            DivisionVal = new DivisionV1DTOValidator();
            LanguageVal = new LanguageV1DTOValidator();
            LocationTypeVal = new LocationTypeV1DTOValidator();
            LocationVal = new LocationV1DTOValidator();
            DepartmentVal = new DepartmentV1DTOValidator();
            ServiceVal = new ServiceV1DTOValidator();
            ProgramVal = new ProgramV1DTOValidator();
            ServiceLanguageVal = new ServiceLanguageAssociationV1DTOValidator();
            ServiceLocationVal = new ServiceLocationAssociationV1DTOValidator();
            ServiceCommunityVal = new ServiceCommunityAssociationV1DTOValidator();
        }

        [Test]
        public void Department_should_not_have_errors()
        {


            var dept = new DepartmentV1DTO();
            dept.DepartmentCode = 1;
            dept.DepartmentName = "some name";
            var deptResult = DepartmentVal.TestValidate(dept);


            deptResult.ShouldNotHaveValidationErrorFor(x => x.DepartmentName);
            deptResult.ShouldNotHaveValidationErrorFor(x => x.DepartmentCode);
        }
        [Test]
        public void Department_should_have_errors()
        {
            var validator = new DepartmentV1DTOValidator();
            validator.ShouldHaveValidationErrorFor(x => x.DepartmentName, null as string);
            validator.ShouldHaveValidationErrorFor(x => x.DepartmentName,  new string('x',31));
        }

        [Test]
        public void Contact_should_not_have_errors()
        {
            var contact = new ContactV1DTO();
            contact.ContactName = "a valid name";
            contact.PhoneNumber = "555-555-5555";
            contact.EmailAddress = "vali@mult.co";
            var Result = ContactVal.TestValidate(contact);


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


        [Test]
        public void Program_should_not_have_errors()
        {
            var program = new ProgramV1DTO();
            program.SponsorName = "a valid name";
            program.OfferType = "1";
            program.ProgramName = "a valid name";
            program.ProgramOfferNumber = "1";
            var Result = ProgramVal.TestValidate(program);


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
            ProgramVal.ShouldHaveValidationErrorFor(x => x.ProgramName, new string('x', 31));

            ProgramVal.ShouldHaveValidationErrorFor(x => x.OfferType, null as string);
            ProgramVal.ShouldHaveValidationErrorFor(x => x.OfferType, new string('x', 21));

            ProgramVal.ShouldHaveValidationErrorFor(x => x.ProgramOfferNumber, null as string);
            ProgramVal.ShouldHaveValidationErrorFor(x => x.ProgramOfferNumber, new string('x', 11));


        }

        [Test]
        public void Location_should_not_have_errors()
        {
            var location= new LocationV1DTO();
            location.LocationName = "a valid name";
            location.BuildingID= "1";
            location.LocationAddress = "506 SW Mill St, Portland, OR 97201";
            location.RoomNumber = "1";
            location.FloorNumber = "1";
            var Result = LocationVal.TestValidate(location);


            Result.ShouldNotHaveValidationErrorFor(x => x.LocationTypeID);
            Result.ShouldNotHaveValidationErrorFor(x => x.LocationName);
            Result.ShouldNotHaveValidationErrorFor(x => x.BuildingID);
            Result.ShouldNotHaveValidationErrorFor(x => x.LocationAddress);
            Result.ShouldNotHaveValidationErrorFor(x => x.RoomNumber);
            Result.ShouldNotHaveValidationErrorFor(x => x.FloorNumber);

        }
        [Test]
        public void Location_should_have_errors()
        {
            LocationVal.ShouldHaveValidationErrorFor(x => x.LocationName, null as string);
            LocationVal.ShouldHaveValidationErrorFor(x => x.LocationName, new string('x', 31));

            LocationVal.ShouldHaveValidationErrorFor(x => x.BuildingID, null as string);

            LocationVal.ShouldHaveValidationErrorFor(x => x.LocationAddress, null as string);
            LocationVal.ShouldHaveValidationErrorFor(x => x.LocationAddress, new string('x', 51));

            LocationVal.ShouldHaveValidationErrorFor(x => x.RoomNumber, null as string);
            LocationVal.ShouldHaveValidationErrorFor(x => x.FloorNumber, null as string);
        }

        [Test]
        public void LocationType_should_not_have_errors()
        {
            var locationType = new LocationTypeV1DTO();
            locationType.LocationTypeName = "valid name";

            var Result = LocationTypeVal.TestValidate(locationType);


            Result.ShouldNotHaveValidationErrorFor(x => x.LocationTypeName);
        }
        [Test]
        public void LocationType_should_have_errors()
        {
            LocationTypeVal.ShouldHaveValidationErrorFor(x => x.LocationTypeName, null as string);
            LocationTypeVal.ShouldHaveValidationErrorFor(x => x.LocationTypeName, new string('x', 31));
        }

        [Test]
        public void Division_should_not_have_errors()
        {
            var division = new DivisionV1DTO();
            division.DivisionName= "valid name";

            division.DivisionCode = 1;

            var Result = DivisionVal.TestValidate(division);

            Result.ShouldNotHaveValidationErrorFor(x => x.DivisionCode);
            Result.ShouldNotHaveValidationErrorFor(x => x.DivisionName);
        }
        [Test]
        public void Division_should_have_errors()
        {
            DivisionVal.ShouldHaveValidationErrorFor(x => x.DivisionName, null as string);
            DivisionVal.ShouldHaveValidationErrorFor(x => x.DivisionName, new string('x', 31));
        }

        [Test]
        public void Language_should_not_have_errors()
        {
            var language = new LanguageV1DTO();
            language.LanguageName = "valid name";

            var Result = LanguageVal.TestValidate(language);


            Result.ShouldNotHaveValidationErrorFor(x => x.LanguageName);
        }
        [Test]
        public void Language_should_have_errors()
        {
            LanguageVal.ShouldHaveValidationErrorFor(x => x.LanguageName, null as string);
            LanguageVal.ShouldHaveValidationErrorFor(x => x.LanguageName, new string('x', 31));
        }

        [Test]
        public void Community_should_not_have_errors()
        {


            var community = new CommunityV1DTO();
            community.CommunityName = "valid name";
            community.CommunityDescription= "A valid description";
            

            var Result = CommunityVal.TestValidate(community);


            Result.ShouldNotHaveValidationErrorFor(x => x.CommunityName);
            Result.ShouldNotHaveValidationErrorFor(x => x.CommunityDescription);
        }
        [Test]
        public void Community_should_have_errors()
        {
            //CommunityVal.ShouldHaveValidationErrorFor(x => x.CommunityID, 0);
            //CommunityVal.ShouldHaveValidationErrorFor(x => x.CommunityID, -1);

            CommunityVal.ShouldHaveValidationErrorFor(x => x.CommunityName, null as string);
            CommunityVal.ShouldHaveValidationErrorFor(x => x.CommunityName, new string('x', 51));

            CommunityVal.ShouldHaveValidationErrorFor(x => x.CommunityDescription, null as string);
            CommunityVal.ShouldHaveValidationErrorFor(x => x.CommunityDescription, new string('x', 51));
        }

        [Test]
        public void ServiceCommunity_should_not_have_errors()
        {

            var ServiceCommunity = new ServiceCommunityAssociationV1DTO();
            ServiceCommunity.ServiceID = 5;
            ServiceCommunity.CommunityID = 2;

            var Result = ServiceCommunityVal.TestValidate(ServiceCommunity);


            Result.ShouldNotHaveValidationErrorFor(x => x.ServiceID);
            Result.ShouldNotHaveValidationErrorFor(x => x.CommunityID);
        }
        [Test]
        public void ServiceCommunity_should_have_errors()
        {

            ServiceCommunityVal.ShouldHaveValidationErrorFor(x => x.CommunityID, 0);
            ServiceCommunityVal.ShouldHaveValidationErrorFor(x => x.CommunityID, -1);


            ServiceCommunityVal.ShouldHaveValidationErrorFor(x => x.ServiceID, 0);
            ServiceCommunityVal.ShouldHaveValidationErrorFor(x => x.ServiceID, -1);

        }

        [Test]
        public void ServiceLanguage_should_not_have_errors()
        {

            var ServiceLanguage = new ServiceLanguageAssociationV1DTO();
            ServiceLanguage.ServiceID = 2;
            ServiceLanguage.LanguageID = 5;

            var Result = ServiceLanguageVal.TestValidate(ServiceLanguage);


            Result.ShouldNotHaveValidationErrorFor(x => x.ServiceID);
            Result.ShouldNotHaveValidationErrorFor(x => x.LanguageID);
        }
        [Test]
        public void ServiceLanguage_should_have_errors()
        {
            //ServiceLanguageVal.ShouldHaveValidationErrorFor(x => x.ServiceLanguageAssociation, 0);
            //ServiceLanguageVal.ShouldHaveValidationErrorFor(x => x.ServiceLanguageAssociation, -1);


            ServiceLanguageVal.ShouldHaveValidationErrorFor(x => x.LanguageID, 0);
            ServiceLanguageVal.ShouldHaveValidationErrorFor(x => x.LanguageID, -1);


            ServiceLanguageVal.ShouldHaveValidationErrorFor(x => x.ServiceID, 0);
            ServiceLanguageVal.ShouldHaveValidationErrorFor(x => x.ServiceID, -1);
        }
        [Test]
        public void ServiceLocation_should_not_have_errors()
        {

            var ServiceLocation = new ServiceLocationAssociationV1DTO();
            ServiceLocation.ServiceID = 1;
            ServiceLocation.LocationID = 2;

            var Result = ServiceLocationVal.TestValidate(ServiceLocation);


            Result.ShouldNotHaveValidationErrorFor(x => x.ServiceID);
            Result.ShouldNotHaveValidationErrorFor(x => x.LocationID);
        }
        [Test]
        public void ServiceLocation_should_have_errors()
        {

            ServiceLocationVal.ShouldHaveValidationErrorFor(x => x.LocationID, 0);
            ServiceLocationVal.ShouldHaveValidationErrorFor(x => x.LocationID, -1);

            ServiceLocationVal.ShouldHaveValidationErrorFor(x => x.ServiceID, 0);
            ServiceLocationVal.ShouldHaveValidationErrorFor(x => x.ServiceID, -1);
        }

        [Test]
        public void Service_should_not_have_errors()
        {
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

            ServiceVal.ShouldNotHaveValidationErrorFor(x => x.ContactDTO, new ContactV1DTO());
            ServiceVal.ShouldHaveChildValidator(x => x.ContactDTO, typeof(ContactV1DTOValidator));

            ServiceVal.ShouldNotHaveValidationErrorFor(x => x.ProgramDTO, new ProgramV1DTO());
            ServiceVal.ShouldHaveChildValidator(x => x.ProgramDTO, typeof(ProgramV1DTOValidator));
         
            ServiceVal.ShouldNotHaveValidationErrorFor(x => x.DepartmentDTO, new DepartmentV1DTO());
            ServiceVal.ShouldHaveChildValidator(x => x.DepartmentDTO, typeof(DepartmentV1DTOValidator));


            ServiceVal.ShouldNotHaveValidationErrorFor(x => x.DivisionDTO, new DivisionV1DTO());
            ServiceVal.ShouldHaveChildValidator(x => x.DivisionDTO, typeof(DivisionV1DTOValidator));

        }
        [Test]
        public void Service_should_have_errors()
        {

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
