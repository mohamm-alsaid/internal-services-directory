using System;
using System.Linq;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using FluentValidation;
using FluentValidation.TestHelper;
using MultCo_ISD_API.V1.Validators;

[TestFixture]
public class CommunityValidatorTest
{
   private CommunityValidator validator;

    [SetUp]
    public void Setup()
    {
        validator = new CommunityValidator();
    }

    [Test]
    public void Should_have_error_when_CommunityID_is_null()
    {
        validator.ShouldHaveValidationErrorFor(x => x.CommunityID, null as int);
    }

    [Test]
    public void Should_have_error_when_CommunityName_is_null()
    {
        validator.ShouldHaveValidationErrorFor(x => x.CommunityName, null as string);
    }

    [Test]
    public void Should_have_error_when_CommunityDescription_is_null()
    {
        validator.ShouldHaveValidationErrorFor(x => x.CommunityDescription, null as string);
    }

}