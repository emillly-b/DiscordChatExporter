﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace DiscordChatExporter.Core.Markdown
{
    internal class LinkNode : MarkdownNode
    {
        public string Url { get; }

        public IReadOnlyList<MarkdownNode> Children { get; }

        public LinkNode(string url, IReadOnlyList<MarkdownNode> children)
        {
            Url = url;
            Children = children;
        }

        public LinkNode(string url)
            : this(url, new[] {new TextNode(url)})
        {
        }

        [ExcludeFromCodeCoverage]
        public override string ToString()
        {
            var childrenFormatted = Children.Count == 1
                ? Children.Single().ToString()
                : "+" + Children.Count;

            return $"<Link> ({childrenFormatted})";
        }
    }
}