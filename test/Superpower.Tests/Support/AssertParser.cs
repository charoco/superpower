﻿// Copyright 2016 Datalust, Superpower Contributors, Sprache Contributors
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at  
//
//     http://www.apache.org/licenses/LICENSE-2.0  
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;
using Xunit;
using Superpower;
using Superpower.Model;
using System.Linq;

namespace Superpower.Tests.Support
{
    static class AssertParser
    {
        public static void SucceedsWithOne<T>(CharParser<T[]> parser, string input, T expectedResult)
        {
            Succeeds(parser, input, t =>
            {
                Assert.Equal(1, t.Count());
                Assert.Equal(expectedResult, t.Single());
            });
        }

        public static void SucceedsWithMany<T>(CharParser<T[]> parser, string input, IEnumerable<T> expectedResult)
        {
            Succeeds(parser, input, t => Assert.True(t.SequenceEqual(expectedResult)));
        }

        public static void SucceedsWithAll(CharParser<char[]> parser, string input)
        {
            SucceedsWithMany(parser, input, input.ToCharArray());
        }

        public static void Succeeds<T>(CharParser<T> parser, string input, Action<T> resultAssertion)
        {
            var t = parser.Parse(input);
            resultAssertion(t);
        }

        public static void SucceedsWith<T>(CharParser<T> parser, string input, T value)
        {
            var t = parser.Parse(input);
            Assert.Equal(value, t);
        }

        public static void Fails<T>(CharParser<T> parser, string input)
        {
            FailsWith(parser, input, f => { });
        }

        public static void FailsAt<T>(CharParser<T> parser, string input, int position)
        {
            FailsWith(parser, input, f => Assert.Equal(position, f.Remainder.Position.Absolute));
        }

        public static void FailsWith<T>(CharParser<T> parser, string input, Action<CharResult<T>> resultAssertion)
        {
            var result = parser.TryParse(input);

            if (result.HasValue)
                Assert.False(result.HasValue, $"Expected failure but succeeded with {result.Value}.");

            resultAssertion(result);
        }

        public static void FailsWithMessage<T>(CharParser<T> parser, string input, string message)
        {
            FailsWith(parser, input, r => { Assert.Equal(message, r.ToString()); });
        }

        public static void SucceedsWithOne<T>(TokenParser<char, T[]> parser, string input, T expectedResult)
        {
            Succeeds(parser, input, t =>
            {
                Assert.Equal(1, t.Count());
                Assert.Equal(expectedResult, t.Single());
            });
        }

        public static void SucceedsWithMany<T>(TokenParser<char, T[]> parser, string input, IEnumerable<T> expectedResult)
        {
            Succeeds(parser, input, t => Assert.True(t.SequenceEqual(expectedResult)));
        }

        public static void SucceedsWithMany(TokenParser<char, Token<char>[]> parser, string input, IEnumerable<char> expectedResult)
        {
            Succeeds(parser, input, t => Assert.True(t.Select(tok => tok.Kind).SequenceEqual(expectedResult)));
        }

        public static void SucceedsWithAll(TokenParser<char, Token<char>[]> parser, string input)
        {
            SucceedsWithMany(parser.Select(t => t.Select(tk => tk.Kind).ToArray()), input, input.ToCharArray());
        }

        public static void Succeeds<T>(TokenParser<char, T> parser, string input, Action<T> resultAssertion)
        {
            var t = parser.Parse(StringAsCharTokenList.Tokenize(input));
            resultAssertion(t);
        }

        public static void SucceedsWith(TokenParser<char, Token<char>> parser, string input, char value)
        {
            Succeeds(parser, input, tok =>
            {
                Assert.Equal(value, tok.Kind);
            });
        }

        public static void SucceedsWith<T>(TokenParser<char, T> parser, string input, T value)
        {
            Succeeds(parser, input, v =>
            {
                Assert.Equal(value, v);
            });
        }

        public static void Fails<T>(TokenParser<char, T> parser, string input)
        {
            FailsWith(parser, input, f => { });
        }

        public static void FailsAt<T>(TokenParser<char, T> parser, string input, int position)
        {
            FailsWith(parser, input, f => Assert.Equal(position, f.Remainder.Position));
        }

        public static void FailsWith<T>(TokenParser<char, T> parser, string input,
            Action<TokenResult<char, T>> resultAssertion)
        {
            var result = parser.TryParse(StringAsCharTokenList.Tokenize((input)));

            if (result.HasValue)
                Assert.False(result.HasValue, $"Expected failure but succeeded with {result.Value}.");

            resultAssertion(result);
        }

        public static void FailsWithMessage<TTokenKind, T>(TokenParser<TTokenKind, T> parser, string input,
            Tokenizer<TTokenKind> tokenizer, string message)
        {
            var result = parser.TryParse(tokenizer.Tokenize(input));
            Assert.Equal(message, result.ToString());
        }

        public static void FailsWithMessage<T>(TokenParser<char, T> parser, string input, string message)
        {
            var result = parser.TryParse(StringAsCharTokenList.Tokenize(input));
            Assert.Equal(message, result.ToString());
        }
    }
}
