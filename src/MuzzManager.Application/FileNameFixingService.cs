namespace MuzzManager.Application
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Core.Interfaces;
    using Core.Models;
    using Microsoft.Extensions.Options;

    public class FileNameFixingService : IFileNameFixingService
    {
        private static readonly char?[] NonLetterCharactersForCapitalization = { null, ' ', '(' };

        private readonly List<Replacement> _replacements;

        public FileNameFixingService(IOptions<List<Replacement>> replacements)
        {
            _replacements = replacements.Value ?? throw new ArgumentNullException(nameof(replacements));
        }

        public string FixName(string fileName)
        {
            var processingFileName = Capitalize(fileName);

            var replacementsForFullName = _replacements.Where(r => r.Scope == ReplacementScope.Full).ToList();
            var replacementsForArtist = _replacements.Where(r => r.Scope == ReplacementScope.Artist).ToList();
            var replacementsForTitle = _replacements.Where(r => r.Scope == ReplacementScope.Title).ToList();

            foreach (var r in replacementsForFullName)
            {
                processingFileName = ReplaceByRegex(processingFileName, r.Pattern, r.ReplacementValue);
            }

            var splittedFileName = processingFileName.Split(" - ");

            if (splittedFileName.Length < 2)
            {
                throw new Exception($"Invalid file name: '{fileName}'");
            }

            var artist = splittedFileName[0];
            var title = splittedFileName[1];

            foreach (var r in replacementsForArtist)
            {
                artist = ReplaceByRegex(artist, r.Pattern, r.ReplacementValue);
            }

            foreach (var r in replacementsForTitle)
            {
                title = ReplaceByRegex(title, r.Pattern, r.ReplacementValue);
            }

            return $"{artist} - {title}";
        }

        private static string Capitalize(string value)
        {
            var charArray = value.ToCharArray();

            for (var i = 0; i < charArray.Length; i++)
            {
                var c = charArray[i];
                var isLower = c >= 97 && c <= 122;

                var previousChar = i == 0 ? (char?) null : charArray[i - 1];
                var afterNonLetterChar = NonLetterCharactersForCapitalization.Contains(previousChar);

                if (isLower && afterNonLetterChar)
                {
                    charArray[i] = (char) (c - 32);
                }
            }

            return new string(charArray);
        }

        private static string ReplaceByRegex(string value, string pattern, string replacement)
        {
            var regex = new Regex(pattern);
            return  regex.Replace(value, replacement);
        }
    }
}