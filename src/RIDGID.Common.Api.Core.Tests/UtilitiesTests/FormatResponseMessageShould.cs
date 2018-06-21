﻿using NUnit.Framework;
using RIDGID.Common.Api.Core.Utilities;
using Shouldly;
using System.Configuration;

namespace RIDGID.Common.Api.Core.Tests.UtilitiesTests
{
    [TestFixture]
    public class FormatResponseMessageShould
    {
        [Test]
        public void IsSnakeCaseReturnsFalseWithNoAppConfigSnakeCaseSetting()
        {
            //--Act/Assert
            FormatResponseMessage.IsSnakeCase().ShouldBeFalse();
        }

        [Test]
        public void IsSnakeCaseReturnsTrueWithAppConfigSnakeCaseSettingTrue()
        {
            //--Arrange
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["snakecase"].Value = "true";
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");

            //--Act/Assert
            FormatResponseMessage.IsSnakeCase().ShouldBeTrue();
        }


        [Test]
        public void IsSnakeCaseReturnsFalseWithAppConfigSnakeCaseSettingFalse()
        {
            //--Arrange
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["snakecase"].Value = "false";
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");

            //--Act/Assert
            FormatResponseMessage.IsSnakeCase().ShouldBeFalse();
        }
    }
}