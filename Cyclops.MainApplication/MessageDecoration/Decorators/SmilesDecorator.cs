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
                    if (!resultPair.IsDelimiter)
                    {
                        var runex = new RunEx(resultPair.String, MessagePartType.Body);
                        runex.Style = run.Style;
                        result.Add(runex);
                    }
                    else
                        result.Add(CreateSmileInline(smiles.First(i => i.Masks.Any(mask => mask == resultPair.String))));
                }
            }

            return result;
        }

        #endregion

        private static Inline CreateSmileInline(ISmile smile)
        {
            var smileImage = new AnimatedImage();
            smileImage.AnimatedBitmap = smile.Bitmap;
            smileImage.Width = smileImage.AnimatedBitmap.Width;
            smileImage.Height = smileImage.AnimatedBitmap.Height;
            smileImage.ToolTip = smile.Masks.First();
            var inline = new InlineUIContainer(smileImage);
            return inline;
        }
    }
}