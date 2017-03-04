﻿// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace Avalonia.Markup.Xaml.Converters
{
#if !OMNIXAML

    using Portable.Xaml.ComponentModel;

    public class IconTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var path = value as string;
            if (path != null)
            {
                return CreateIconFromPath(context, path);
            }
            var bitmap = value as IBitmap;
            if (bitmap != null)
            {
                return new WindowIcon(bitmap);
            }
            throw new NotSupportedException();
        }

        private WindowIcon CreateIconFromPath(ITypeDescriptorContext context, string path)
        {
            var uri = new Uri(path, UriKind.RelativeOrAbsolute);
            var baseUri = GetBaseUri(uri, context);
            var scheme = uri.IsAbsoluteUri ? uri.Scheme : "file";

            switch (scheme)
            {
                case "file":
                    return new WindowIcon(path);
                default:
                    var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
                    return new WindowIcon(assets.Open(uri, baseUri));
            }
        }

        private Uri GetBaseUri(Uri uri, ITypeDescriptorContext context)
        {
            if (uri.IsAbsoluteUri)
            {
                return null;
            }

            throw new NotImplementedException("Relative base uri Not implemented!");
            //object result;
            //context.ParsingDictionary.TryGetValue("Uri", out result);
            //return result as Uri;
        }
    }

#else

    using OmniXaml.TypeConversion;
    using System.Reflection;

    class IconTypeConverter : ITypeConverter
    {
        public bool CanConvertFrom(IValueContext context, Type sourceType)
        {
            return sourceType == typeof(string) || typeof(IBitmap).GetTypeInfo().IsAssignableFrom(sourceType.GetTypeInfo());
        }

        public bool CanConvertTo(IValueContext context, Type destinationType)
        {
            return false;
        }

        public object ConvertFrom(IValueContext context, CultureInfo culture, object value)
        {
            var path = value as string;
            if (path != null)
            {
                return CreateIconFromPath(context, path); 
            }
            var bitmap = value as IBitmap;
            if (bitmap != null)
            {
                return new WindowIcon(bitmap);
            }
            throw new NotSupportedException();
        }

        private WindowIcon CreateIconFromPath(IValueContext context, string path)
        {
            var uri = new Uri(path, UriKind.RelativeOrAbsolute);
            var baseUri = GetBaseUri(context);
            var scheme = uri.IsAbsoluteUri ? uri.Scheme : "file";

            switch (scheme)
            {
                case "file":
                    return new WindowIcon(path);
                default:
                    var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
                    return new WindowIcon(assets.Open(uri, baseUri));
            }
        }

        public object ConvertTo(IValueContext context, CultureInfo culture, object value, Type destinationType)
        {
            throw new NotImplementedException();
        }

        private Uri GetBaseUri(IValueContext context)
        {
            object result;
            context.ParsingDictionary.TryGetValue("Uri", out result);
            return result as Uri;
        }
    }
#endif
}
