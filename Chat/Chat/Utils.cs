using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat
{
    class Utils
    {

        #region string to colored
        public static List<Tuple<Color, string>> StringToColoredString(string content, Color defaultColor)
        {
            List<Tuple<Color, string>> parts = new List<Tuple<Color, string>>();

            int advancement = 0;
            int index = 0;
            bool inColored = false;

            int beginColor, endColor;
            string color;

            while (advancement < content.Length)
            {
                index = content.IndexOf('*', index);

                if (inColored)
                {
                    if (index == -1) break;
                    if (index < content.Length - 7)
                    {
                        ++index;
                        if (content.ElementAt(index) == '*')
                        {
                            inColored = false;

                            if (index - advancement > 0)
                            {
                                beginColor = content.IndexOf('(', index) + 1;
                                endColor = content.IndexOf(')', index);

                                if (beginColor > 4 && endColor > 4) // after ** and **
                                {
                                    color = content.Substring(beginColor, endColor - beginColor).Trim();

                                    Color col;
                                    if (char.IsLetter(color.First()))
                                    {
                                        col = Color.FromName(color);
                                    }
                                    else
                                    {
                                        string[] colorPart = color.Split(',');

                                        col = (colorPart.Length == 3)
                                          ? Color.FromArgb(Convert.ToInt32(colorPart[0]),
                                                    Convert.ToInt32(colorPart[1]),
                                                    Convert.ToInt32(colorPart[2]))
                                          : Color.FromArgb(Convert.ToInt32(colorPart[3]),
                                                Convert.ToInt32(colorPart[0]),
                                                Convert.ToInt32(colorPart[1]),
                                                Convert.ToInt32(colorPart[2]));
                                    }

                                    parts.Add(new Tuple<Color, string>(col,
                                        content.Substring(advancement, index - advancement - 1))
                                        );

                                    advancement = endColor + 1;
                                }
                                else
                                {
                                    advancement = index + 1;
                                }
                            }
                        }
                        else
                        {
                            advancement = index + 1;
                        }
                    }
                }
                else
                {
                    if (index == -1) break;
                    if (index < content.Length - 7)
                    {
                        ++index;
                        if (content.ElementAt(index) == '*')
                        {
                            inColored = true;
                            if (index - advancement - 1 > 0)
                            {
                                parts.Add(new Tuple<Color, string>(defaultColor,
                                    content.Substring(advancement, index - advancement - 1))
                                    );
                            }

                            advancement = index + 1;
                        }
                    }
                }

                ++index;
            }

            if (content.Length - advancement > 0)
            {
                parts.Add(new Tuple<Color, string>(defaultColor,
                    content.Substring(advancement, content.Length - advancement))
                    );
            }

            return parts;
        }
        public static List<Tuple<Color, string>> StringToColoredString(string content)
        {
            return StringToColoredString(content, Color.Black);
        }

        public static bool IsEmptyProperty(string property)
        {
            if (property.Equals("")) return true;
            if (property.Equals(" ")) return true;

            for (int i = 0; i < property.Length; ++i)
            {
                if (!SpaceLikeCharacter(property[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool SpaceLikeCharacter(char ch)
        {
            return ch == ' ' || ch == '\n' || ch == '\t' || ch == '\r';
        }

        #endregion
    }
}
