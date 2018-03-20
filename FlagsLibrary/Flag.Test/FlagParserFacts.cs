﻿using System;
using Xunit;

namespace Flag.Test
{
    public class FlagParserFacts
    {
        [Fact]
        public void should_parse_success_and_get_flag_value_when_has_flag_full_name()
        {
            var fullName = "flag";
            var description = "the first flag";

            ArgsParser parser = new ArgsParserBuilder().AddFlagOption(fullName, null, description).Build();

            ArgsParsingResult result = parser.Parser(new[] { "--flag" });

            Assert.True(result.IsSuccess);
            Assert.True(result.GetFlagValue("--flag"));
            Assert.Null(result.Error);
        }

        [Fact]
        void should_success_and_get_flag_value_when_add_flag_with_description_is_null()
        {
            var fullName = "flag";
            var abbreviation = 'f';
            var parser = new ArgsParserBuilder().AddFlagOption(fullName, abbreviation, null).Build();

            ArgsParsingResult result = parser.Parser(new[] {"--flag"});
            Assert.True(result.IsSuccess);
            Assert.True(result.GetFlagValue("-f"));
        }

        [Fact]
        public void should_parse_success_and_can_get_flag_value_when_flag_has_abbreviation_name()
        {
            var abbrName = 'f';
            var description = "the first flag";

            ArgsParser parser = new ArgsParserBuilder().AddFlagOption(null, abbrName, description).Build();

            ArgsParsingResult result = parser.Parser(new[] { "-f" });
            Assert.True(result.IsSuccess);
            Assert.True(result.GetFlagValue("-f"));
        }

        [Fact]
        void should_success_and_get_flag_value_when_add_flag_with_full_name_is_null()
        {
            var abbreviation = 'f';
            var description = "the first flag";

            var parser = new ArgsParserBuilder().AddFlagOption(null, abbreviation, description).Build();

            ArgsParsingResult result = parser.Parser(new[] { "-f" });

            Assert.True(result.IsSuccess);
            Assert.True(result.GetFlagValue("-f"));
            Assert.Null(result.Error);
        }

        [Fact]
        void should_success_and_get_flag_value_when_add_flag_with_legal_parameter()
        {
            var fullName = "flag";
            char? abbreviation = 'f';
            var description = "the first flag";
            var parser = new ArgsParserBuilder().AddFlagOption(fullName, abbreviation, description).Build();

            ArgsParsingResult result = parser.Parser(new[] { "--flag" });

            Assert.True(result.IsSuccess);
            Assert.True(result.GetFlagValue("-f"));
        }

        [Fact]
        void should_throw_exception_when_add_flag_with_abbreviation_and_full_name_are_all_null()
        {
            var description = "the first flag";

            Assert.Throws<ArgumentNullException>(() => new ArgsParserBuilder().AddFlagOption(null, null, description).Build());
        }

        [Fact]
        public void should_parse_success_and_can_get_flag_value_when_flag_has_two_valid_names()
        {
            var fullName = "flag";
            var abbrName = 'f';
            var description = "the first flag";

            ArgsParser parser = new ArgsParserBuilder().AddFlagOption(fullName, abbrName, description).Build();

            ArgsParsingResult result = parser.Parser(new[] { "-f" });
            Assert.True(result.IsSuccess);
            Assert.True(result.GetFlagValue("--flag"));
            Assert.Null(result.Error);

            ArgsParsingResult result2 = parser.Parser(new[] { "--flag" });
            Assert.True(result2.IsSuccess);
            Assert.True(result2.GetFlagValue("-f"));
            Assert.Null(result2.Error);
        }

        [Fact]
        public void should_throw_exception_when_add_flag_with_empty_full_name_and_null_abbr_name()
        {
            Assert.Throws<ArgumentException>(() => new ArgsParserBuilder().AddFlagOption("", null, "description"));
        }

        [Fact]
        public void should_throw_exception_when_add_flag_with_null_full_name_and_empty_abbr_name()
        {
            Assert.Throws<ArgumentException>(() => new ArgsParserBuilder().AddFlagOption(null, default(char), "description"));
        }

        [Fact]
        public void should_throw_exception_when_add_flag_with_invalid_full_name_or_abbr_name()
        {
            Assert.Throws<ArgumentException>(() => new ArgsParserBuilder().AddFlagOption("flag", '3', "description"));
            Assert.Throws<ArgumentException>(() => new ArgsParserBuilder().AddFlagOption("-flag", 'f', "description"));
        }

        [Fact]
        public void should_return_false_and_get_error_code_when_parse_parameter_is_invalid()
        {
            var fullName = "flag";
            var abbrName = 'f';
            var description = "the first flag";

            ArgsParser parser = new ArgsParserBuilder().AddFlagOption(fullName, abbrName, description).Build();
            ArgsParsingResult result = parser.Parser(new[] { "-f", "-flag" });

            Assert.False(result.IsSuccess);
            Assert.False(result.GetFlagValue("--flag"));
            Assert.Equal(ParsingErrorCode.InvalidOptionName, result.Error.Code);
            Assert.Equal("-flag", result.Error.Trigger);
        }

        [Fact]
        public void should_return_false_and_get_unfined_error_code_when_parse_parameter_is_undefined()
        {
            var fullName = "flag";
            var abbrName = 'f';
            var description = "the first flag";

            ArgsParser parser = new ArgsParserBuilder().AddFlagOption(fullName, abbrName, description).Build();
            ArgsParsingResult result = parser.Parser(new[] { "-f", "-v" });

            Assert.False(result.IsSuccess);
            Assert.False(result.GetFlagValue("--flag"));
            Assert.Equal(ParsingErrorCode.UndefinedOption, result.Error.Code);
            Assert.Equal("-v", result.Error.Trigger);
        }

        [Fact]
        public void should_return_false_when_get_flag_value_with_wrong_parameters()
        {
            var fullName = "flag";
            var abbrName = 'f';
            var description = "the first flag";

            ArgsParser parser = new ArgsParserBuilder().AddFlagOption(fullName, abbrName, description).Build();

            ArgsParsingResult result = parser.Parser(new[] { "-f" });

            Assert.True(result.IsSuccess);
            Assert.True(result.GetFlagValue("--flag"));
            Assert.False(result.GetFlagValue("--f"));
            Assert.False(result.GetFlagValue("-flag"));
            Assert.False(result.GetFlagValue("-v"));
        }

    }
}