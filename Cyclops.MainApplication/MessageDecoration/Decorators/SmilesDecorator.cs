using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using Cyclops.Core;
using Cyclops.Core.Smiles;
using Cyclops.MainApplication.Controls;

namespace Cyclops.MainApplication.MessageDecoration.Decorators
{
    public class SmilesDecorator : IMessageDecorator
    {
        #region Implementation of IMessageDecorator

        /// <summary>
        /// Transform collection of inlines
        /// </summary>
        public List<Inline> Decorate(IConferenceMessage msg, List<Inline> inlines)
        {
            var smiles = ApplicationContext.Current.SmilePacks.SelectMany(i => i.Smiles);
            List<Inline> result = new List<Inline>();
            int smilesInsertedCount = 0;
            foreach (var inline in inlines)
            {
                if (!(inline is RunEx) || ((RunEx)inline).MessagePartType != MessagePartType.Body)
                {
                    result.Add(inline);
                    continue;
                }
                RunEx run = inline as RunEx;
                var parts = run.Text.SplitAndIncludeDelimiters(smiles.SelectMany(i => i.Masks).Distinct().ToArray()).ToArray();
                foreach (var resultPair in parts)
                {
                    if (!resultPair.IsDelimiter || smilesInsertedCount >= ApplicationContext.Current.Settings.SmilesLimitInMessage)
                    {
                        var runex = new RunEx(resultPair.String, MessagePartType.Body);
                        runex.SetResourceReference(FrameworkContentElement.StyleProperty, DecoratorsStyles.CommonMessageStyle);
                        result.Add(runex);
                    }
                    else
                    {
                        result.Add(CreateSmileInline(smiles.First(i => i.Masks.Any(mask => mask == resultPair.String))));
                        smilesInsertedCount++;
                    }
                }
            }

            return result;
        }

        #endregion

        private static Inline CreateSmileInline(ISmile smile)
        {
            var smileImage = new AnimatedImage();
            smileImage.AnimatedBitmap = smile.Bitmap;
            smileImage.Stretch = System.Windows.Media.Stretch.None;
            smileImage.Width = smileImage.AnimatedBitmap.Width;
            smileImage.Height = smileImage.AnimatedBitmap.Height;
            smileImage.ToolTip = smile.Masks.First();
            smileImage.StaticByDefault = ApplicationContext.Current.Settings.IsSmilesAnimated;
            var inline = new InlineUIContainer(smileImage);
            return inline;
        }
    }
}