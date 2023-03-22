using System.Text.RegularExpressions;

namespace Point.Application.Attributes
{
    public class ContentValidation : ValidationAttribute
    {
        private readonly int _maxLength;

        public ContentValidation(int maxLength)
        {
            _maxLength = maxLength;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var content = value as string;
            if (content != null)
            {
                if (content.Length > _maxLength)
                    return new ValidationResult($"Max length {_maxLength}");

                if (!ShouldNotContainHtmlScriptTag(content))
                    return new ValidationResult("Field should not contain script tags");

                if (!ShouldNotContainBlockedTags(content))
                    return new ValidationResult("Field should not contain blocked tags");

                if (!ShouldContainAllowedAttributesAndTags(content))
                    return new ValidationResult("Field should not contain blocked tags or attributes");
            }

            return ValidationResult.Success;
        }

        private bool ShouldNotContainHtmlScriptTag(string content)
        {
            var scriptRegex = new Regex(@"<script\b[^<]*(?:(?!</script>)<[^<]*)*</script>",
                RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);

            return scriptRegex.IsMatch(content);
        }

        private bool ShouldNotContainBlockedTags(string content)
        {
            var blockedTagsRegex =
                    new Regex(@"<\s*(script|iframe|frame|object|embed|applet|meta|link|style|base|form|onmouseover|alert|onfocus|autofocus)\b",
                RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);

            return blockedTagsRegex.IsMatch(content);
        }

        private bool ShouldContainAllowedAttributesAndTags(string content)
        {
            var allowedAttributes = new List<string> { "href", "title", "src", "alt" };
            var allowedTags = new List<string> { "p", "a", "strong", "em", "u", "ul", "ol", "li", "img" };

            string allowedAttributesPattern = string.Join("|", allowedAttributes.Select(attr => $"(?i){attr}"));

            string allowedTagsPattern = string.Join("|", allowedTags.Select(tag => $"<{tag}(\\s+(?i)({allowedAttributesPattern})=\".*?\")*\\s*/?>"));

            string pattern = $@"^({allowedTagsPattern}|\s)*$";

            return Regex.IsMatch(content, pattern, RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }
    }
}
