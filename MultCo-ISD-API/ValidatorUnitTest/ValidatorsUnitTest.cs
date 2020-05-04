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

        /*
			 * contant
			 * community
			 * division
			 * language
			 * locationType
			 * Location
			 * ServiceCommunityAssociation
			 * Program
			 * ServiceLanguageAssociation
			 * Department
			 * ServiceLocationAssociation
			 * Service
			 */
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
        private ServiceLocationAssociationValidator serviceLocationVal;
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
            serviceLocationVal = new ServiceLocationAssociationValidator();
            ServiceCommunityVal = new ServiceCommunityAssociationValidator();
        }
        [Test]
        public void should_not_have_errors()
        {

            Department_should_not_have_errors();

            Contact_should_not_have_errors();
            //TODO: create a unit test to check if the validators have been run during a post request (maybe in controllers unit test)
            //TODO: create a unit test to check the return message from creating a post request with valid input (maybe in controllers unit test)
            //TODO: create a unit test to check the return message from creating a post request with invalid input (maybe in controller unit test)




        }
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
        public void Contact_should_not_have_errors()
        {

            // -- SUCESS contact init..
            var contact = new Contact();
            contact.ContactName = "valid name";
            contact.EmailAddress = "valid@valid.com";
            contact.PhoneNumber = "123-456-7891";

            var contactResult = ContactVal.TestValidate(contact);

            // assertions -- cont
            contactResult.ShouldNotHaveValidationErrorFor(x => x.EmailAddress);
            contactResult.ShouldNotHaveValidationErrorFor(x => x.PhoneNumber);
            contactResult.ShouldNotHaveValidationErrorFor(x => x.ContactName);
        }



    }
}
