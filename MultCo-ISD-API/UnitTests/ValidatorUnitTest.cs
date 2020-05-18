using System;
using System.Collections.Generic;
using System.Text;
using MultCo_ISD_API.V1.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentValidation;
using FluentValidation.TestHelper;
using MultCo_ISD_API.V1.Validators;

namespace ValidatorUnitTest
{

    [TestClass]
    public class ValidatorsUnitTest
    {
        private ContactV1DTOValidator ContactVal = new ContactV1DTOValidator();
        private CommunityV1DTOValidator CommunityVal = new CommunityV1DTOValidator();
        private DivisionV1DTOValidator DivisionVal = new DivisionV1DTOValidator();
        private LanguageV1DTOValidator LanguageVal = new LanguageV1DTOValidator();
        private LocationTypeV1DTOValidator LocationTypeVal = new LocationTypeV1DTOValidator();
        private LocationV1DTOValidator LocationVal = new LocationV1DTOValidator();
        private DepartmentV1DTOValidator DepartmentVal = new DepartmentV1DTOValidator();
        private ServiceV1DTOValidator ServiceVal = new ServiceV1DTOValidator();
        private ProgramV1DTOValidator ProgramVal = new ProgramV1DTOValidator();
        private ServiceLanguageAssociationV1DTOValidator ServiceLanguageVal = new ServiceLanguageAssociationV1DTOValidator();
        private ServiceLocationAssociationV1DTOValidator ServiceLocationVal = new ServiceLocationAssociationV1DTOValidator();
        private ServiceCommunityAssociationV1DTOValidator ServiceCommunityVal = new ServiceCommunityAssociationV1DTOValidator();


        [TestMethod]
        public void Department_should_not_have_errors()
        {
            var dept = new DepartmentV1DTO();
            dept.DepartmentCode = 1;
            dept.DepartmentName = "some name";
            var deptResult = DepartmentVal.TestValidate(dept);

            deptResult.ShouldNotHaveValidationErrorFor(x => x.DepartmentName);
            deptResult.ShouldNotHaveValidationErrorFor(x => x.DepartmentCode);
        }
        [TestMethod]
        public void Department_should_have_errors()
        {
            var validator = new DepartmentV1DTOValidator();

            validator.ShouldHaveValidationErrorFor(x => x.DepartmentName, new string('x', 256));
            
        }

        [TestMethod]
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
        [TestMethod]
        public void Contact_should_have_errors()
        {

            ContactVal.ShouldHaveValidationErrorFor(x => x.ContactName, new string('x', 256));


            // tests two cases
            ContactVal.ShouldHaveValidationErrorFor(x => x.PhoneNumber, new string('1', 256));


            ContactVal.ShouldHaveValidationErrorFor(x => x.EmailAddress, new string('a', 256));


        }


        [TestMethod]
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
        [TestMethod]
        public void Program_should_have_errors()
        {

            ProgramVal.ShouldHaveValidationErrorFor(x => x.SponsorName, new string('x', 256));


            ProgramVal.ShouldHaveValidationErrorFor(x => x.ProgramName, new string('x', 256));


            ProgramVal.ShouldHaveValidationErrorFor(x => x.OfferType, new string('x', 256));


            ProgramVal.ShouldHaveValidationErrorFor(x => x.ProgramOfferNumber, new string('x', 256));


        }

        [TestMethod]
        public void Location_should_not_have_errors()
        {
            var location = new LocationV1DTO();
            location.LocationName = "a valid name";
            location.BuildingId = "1";
            location.LocationAddress = "506 SW Mill St, Portland, OR 97201";
            location.RoomNumber = "1";
            location.FloorNumber = "1";
            var Result = LocationVal.TestValidate(location);


            Result.ShouldNotHaveValidationErrorFor(x => x.LocationTypeId);
            Result.ShouldNotHaveValidationErrorFor(x => x.LocationName);
            Result.ShouldNotHaveValidationErrorFor(x => x.BuildingId);
            Result.ShouldNotHaveValidationErrorFor(x => x.LocationAddress);
            Result.ShouldNotHaveValidationErrorFor(x => x.RoomNumber);
            Result.ShouldNotHaveValidationErrorFor(x => x.FloorNumber);

        }
        [TestMethod]
        public void Location_should_have_errors()
        {
            LocationVal.ShouldHaveValidationErrorFor(x => x.LocationName, new string('x', 256));

            LocationVal.ShouldHaveValidationErrorFor(x => x.BuildingId, new string('x', 256));

            LocationVal.ShouldHaveValidationErrorFor(x => x.LocationAddress, new string('x', 256));

            LocationVal.ShouldHaveValidationErrorFor(x => x.RoomNumber, new string('x', 256));

            LocationVal.ShouldHaveValidationErrorFor(x => x.FloorNumber, new string('x', 256));

        }

        [TestMethod]
        public void LocationType_should_not_have_errors()
        {
            var locationType = new LocationTypeV1DTO();
            locationType.LocationTypeName = "valid name";

            var Result = LocationTypeVal.TestValidate(locationType);

            Result.ShouldNotHaveValidationErrorFor(x => x.LocationTypeName);
        }
        [TestMethod]
        public void LocationType_should_have_errors()
        {
            LocationTypeVal.ShouldHaveValidationErrorFor(x => x.LocationTypeName, new string('x', 256));
        }

        [TestMethod]
        public void Division_should_not_have_errors()
        {
            var division = new DivisionV1DTO();
            division.DivisionName = "valid name";

            division.DivisionCode = 1;

            var Result = DivisionVal.TestValidate(division);

            Result.ShouldNotHaveValidationErrorFor(x => x.DivisionCode);
            Result.ShouldNotHaveValidationErrorFor(x => x.DivisionName);
        }
        [TestMethod]
        public void Division_should_have_errors()
        {
            DivisionVal.ShouldHaveValidationErrorFor(x => x.DivisionName, new string('x', 256));
        }

        [TestMethod]
        public void Language_should_not_have_errors()
        {
            var language = new LanguageV1DTO();
            language.LanguageName = "valid name";

            var Result = LanguageVal.TestValidate(language);

            Result.ShouldNotHaveValidationErrorFor(x => x.LanguageName);
        }
        [TestMethod]
        public void Language_should_have_errors()
        {

            LanguageVal.ShouldHaveValidationErrorFor(x => x.LanguageName, new string('x', 256));
        }

        [TestMethod]
        public void Community_should_not_have_errors()
        {


            var community = new CommunityV1DTO();
            community.CommunityName = "valid name";
            community.CommunityDescription = "A valid description";


            var Result = CommunityVal.TestValidate(community);

            Result.ShouldNotHaveValidationErrorFor(x => x.CommunityName);
            Result.ShouldNotHaveValidationErrorFor(x => x.CommunityDescription);
        }
        [TestMethod]
        public void Community_should_have_errors()
        {

            CommunityVal.ShouldHaveValidationErrorFor(x => x.CommunityName, new string('x', 256));


            CommunityVal.ShouldHaveValidationErrorFor(x => x.CommunityDescription, new string('x', 256));
        }

        [TestMethod]
        public void ServiceCommunity_should_not_have_errors()
        {

            var ServiceCommunity = new ServiceCommunityAssociationV1DTO();
            ServiceCommunity.ServiceId = 5;
            ServiceCommunity.CommunityId = 2;

            var Result = ServiceCommunityVal.TestValidate(ServiceCommunity);


            Result.ShouldNotHaveValidationErrorFor(x => x.ServiceId);
            Result.ShouldNotHaveValidationErrorFor(x => x.CommunityId);
        }
        [TestMethod]
        public void ServiceCommunity_should_have_errors()
        {

            ServiceCommunityVal.ShouldHaveValidationErrorFor(x => x.CommunityId, 0);
            ServiceCommunityVal.ShouldHaveValidationErrorFor(x => x.CommunityId, -1);


            ServiceCommunityVal.ShouldHaveValidationErrorFor(x => x.ServiceId, 0);
            ServiceCommunityVal.ShouldHaveValidationErrorFor(x => x.ServiceId, -1);

        }

        [TestMethod]
        public void ServiceLanguage_should_not_have_errors()
        {

            var ServiceLanguage = new ServiceLanguageAssociationV1DTO();
            ServiceLanguage.ServiceId = 2;
            ServiceLanguage.LanguageId = 5;

            var Result = ServiceLanguageVal.TestValidate(ServiceLanguage);


            Result.ShouldNotHaveValidationErrorFor(x => x.ServiceId);
            Result.ShouldNotHaveValidationErrorFor(x => x.LanguageId);
        }
        [TestMethod]
        public void ServiceLanguage_should_have_errors()
        {

            ServiceLanguageVal.ShouldHaveValidationErrorFor(x => x.LanguageId, 0);
            ServiceLanguageVal.ShouldHaveValidationErrorFor(x => x.LanguageId, -1);


            ServiceLanguageVal.ShouldHaveValidationErrorFor(x => x.ServiceId, 0);
            ServiceLanguageVal.ShouldHaveValidationErrorFor(x => x.ServiceId, -1);
        }
        [TestMethod]
        public void ServiceLocation_should_not_have_errors()
        {

            var ServiceLocation = new ServiceLocationAssociationV1DTO();
            ServiceLocation.ServiceId = 1;
            ServiceLocation.LocationId = 2;

            var Result = ServiceLocationVal.TestValidate(ServiceLocation);


            Result.ShouldNotHaveValidationErrorFor(x => x.ServiceId);
            Result.ShouldNotHaveValidationErrorFor(x => x.LocationId);
        }
        [TestMethod]
        public void ServiceLocation_should_have_errors()
        {

            ServiceLocationVal.ShouldHaveValidationErrorFor(x => x.LocationId, 0);
            ServiceLocationVal.ShouldHaveValidationErrorFor(x => x.LocationId, -1);

            ServiceLocationVal.ShouldHaveValidationErrorFor(x => x.ServiceId, 0);
            ServiceLocationVal.ShouldHaveValidationErrorFor(x => x.ServiceId, -1);
        }

        [TestMethod]
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
        [TestMethod]
        public void Service_should_have_errors()
        {

            ServiceVal.ShouldHaveValidationErrorFor(x => x.ProgramId, 0);
            ServiceVal.ShouldHaveValidationErrorFor(x => x.ProgramId, -1);

            ServiceVal.ShouldHaveValidationErrorFor(x => x.DivisionId, 0);
            ServiceVal.ShouldHaveValidationErrorFor(x => x.DivisionId, -1);

            ServiceVal.ShouldHaveValidationErrorFor(x => x.ContactId, 0);
            ServiceVal.ShouldHaveValidationErrorFor(x => x.ContactId, -1);

            ServiceVal.ShouldHaveValidationErrorFor(x => x.ServiceName, new string('x', 256));

            ServiceVal.ShouldHaveValidationErrorFor(x => x.ServiceDescription, new string('x', 6001));

            ServiceVal.ShouldHaveValidationErrorFor(x => x.ExecutiveSummary, new string('x', 6001));

            ServiceVal.ShouldHaveValidationErrorFor(x => x.ServiceArea, new string('x', 256));

            ServiceVal.ShouldHaveValidationErrorFor(x => x.EmployeeConnectMethod, new string('x', 256));

            ServiceVal.ShouldHaveValidationErrorFor(x => x.CustomerConnectMethod, new string('x', 256));


        }
    }
}