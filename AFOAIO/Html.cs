using mshtml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace AFOAIO
{
    public class Html : Tools
    {
        #region Fields
        private WebRequest Rq;
        private bool answer_bool;
        private string HTML;
        private DateTime dateTime;
        private string[] Range;
        private string piece1 = "class=\"", piece2 = "\"";
        private WebBrowser browser;
        private List<HtmlElement> elements;
        private readonly string AnotherKey = "myAnotherKey";
        #endregion
        #region Functions
        #region MainFuntions
        public void H_jsExecute(string js, HtmlDocument doc)
        {
            js = js.Replace(@"\\", "");
            HtmlElement scriptElement = doc.CreateElement("script");
            IHTMLScriptElement element = (IHTMLScriptElement)scriptElement.DomElement;
            element.text = "" + js + "";
            doc.GetElementsByTagName("head")[0].AppendChild(scriptElement);
        }
        public HtmlElement H_GetElement(string Html, string ElementType)
        {
            Html = ChangeMyClass(Html.Replace(AnotherKey + "-", "class=\"").Replace("-" + AnotherKey, "\""), AnotherKey);
            HtmlElement htmlElement = null;
            WebBrowser browser = H_StringToBrowser(Html);
            foreach (HtmlElement element in browser.Document.GetElementsByTagName(NonTurkhis(ElementType.Split('.')[0])))
            {
                string text = element.OuterHtml.Substring(0, element.OuterHtml.IndexOf(">"));
                if (text.Contains(AnotherKey))
                {
                    string @class = text.Substring(text.IndexOf(AnotherKey + "-") + (AnotherKey + "-").Length);
                    @class = @class.Substring(0, @class.IndexOf("-" + AnotherKey));
                    if (NonTurkhis((element.TagName + @class).ToLower()) == NonTurkhis(ElementType.ToLower()))
                    {
                        if (element.CanHaveChildren) ParserElement(element, ElementType);
                        htmlElement = element;
                        break;
                    }
                }
            }
            return htmlElement;
        }
        public List<HtmlElement> H_GetElements(string Html, string ElementType)
        {
            Html = ChangeMyClass(Html.Replace(AnotherKey + "-", "class=\"").Replace("-" + AnotherKey, "\""), AnotherKey);
            elements = new List<HtmlElement>();
            browser = H_StringToBrowser(Html);
            foreach (HtmlElement element in browser.Document.GetElementsByTagName(NonTurkhis(ElementType.Split('.')[0])))
            {
                string text = element.OuterHtml.Substring(0, element.OuterHtml.IndexOf(">"));
                if (text.Contains(AnotherKey))
                {
                    string @class = text.Substring(text.IndexOf(AnotherKey + "-") + (AnotherKey + "-").Length);
                    @class = @class.Substring(0, @class.IndexOf("-" + AnotherKey));
                    if (NonTurkhis((element.TagName + @class).ToLower()) == NonTurkhis(ElementType.ToLower()))
                    {
                        if (element.CanHaveChildren) ParserElement(element, ElementType);
                        elements.Add(element);
                    }
                }
            }
            return elements;
        }
        private void ParserElement(HtmlElement element, string ElementType)
        {
            foreach (HtmlElement children in element.Children)
            {
                element.AppendChild(children);
                if (children.CanHaveChildren) ParserElement(children, ElementType);
            }
        }
        public HtmlElement H_GetElement(HtmlDocument htmlDocument, string ElementType)
        {
            string js = @"
                        var newelement = $('" + ElementType + @"').cloneNode(true);;
                        newelement.attr('id','MyElement');
                        document.body.appendChild(newelement);
                        ".Replace(@"\\", "");
            HtmlElement scriptElement = htmlDocument.CreateElement("script");
            ((mshtml.IHTMLScriptElement)scriptElement.DomElement).text = js;
            htmlDocument.GetElementsByTagName("head")[0].AppendChild(scriptElement);
            HtmlElement myDiv = htmlDocument.GetElementById("MyElement");
            return myDiv;
        }
        public bool H_IsLoaded(string Search, ref WebBrowser webBrowser, int time)
        {
            Temizle();
            dateTime = DateTime.Now.AddSeconds(time);
            Range = Search.Split("|"[0]);
            while ((answer_bool == false) && (dateTime > DateTime.Now))
            {
                Application.DoEvents();
                Application.DoEvents();
                if (webBrowser.Document == null || webBrowser.Document.Body == null)
                {
                    continue;
                }
                HTML = webBrowser.Document.Body.OuterHtml;
                for (int i = 0; i < Range.Length; i++)
                {
                    Application.DoEvents();
                    Application.DoEvents();
                    if (HTML.Contains(Range[i]))
                    {
                        answer_bool = true;
                        break;
                    }
                }
            }
            return answer_bool;
        }
        public string H_GetHtml(string url)
        {
            Rq = WebRequest.Create(url);
            return (new StreamReader(Rq.GetResponse().GetResponseStream())).ReadToEnd();
        }
        public WebBrowser H_StringToBrowser(string Html)
        {
            WebBrowser browser = new WebBrowser
            {
                ScriptErrorsSuppressed = true,
                DocumentText = Html
            };
            browser.Document.OpenNew(true);
            browser.Document.Write(Html);
            browser.Refresh();
            return browser;
        }
        public void H_IsOpen(WebBrowser webBrowser)
        {
            while (true)
            {
                if (webBrowser.ReadyState == WebBrowserReadyState.Complete) break;
                Application.DoEvents();
                Application.DoEvents();
            }
        }
        public string H_Translate(string LanguagePair, string Text)
        {
            string url = String.Format("http://www.google.com/translate_t?hl=en&ie=UTF8&text={0}&langpair={1}", Text, LanguagePair);
            using (WebClient webClient = new WebClient() { Encoding = System.Text.Encoding.GetEncoding(1254) })
            {
                return T_HtmlTextClear(H_GetElement(H_StringToBrowser(webClient.DownloadString(url)).Document, "span.tlid-translation.translation span").InnerText);
            }
        }
        #endregion
        #region Support Functions
        private void Temizle()
        {
            answer_bool = false;
            HTML = null;
            Range = null;
        }
        private string ChangeMyClass(string html, string AnotherKey)
        {
            List<string> pieces = new List<string>();
            while (html.Contains(piece1))
            {
                pieces.Add(html.Substring(0, (html.IndexOf(piece1))));
                int l = html.IndexOf(piece1);
                html = html.Substring(l + piece1.Length);
                pieces.Add(html.Substring(0, html.IndexOf(piece2)));
                html = html.Substring(html.IndexOf(piece2) + piece2.Length);
            }
            pieces.Add(html);
            string newhtml = string.Empty;
            for (int i = 0; i < pieces.Count; i++)
            {
                if (i != 0)
                {
                    if (i % 2 == 0)
                        newhtml += "-" + AnotherKey + pieces[i];
                    else if (i % 2 == 1)
                        newhtml += AnotherKey + "-" + ((pieces[i] != "") ?((pieces[i][0] != '.') ? "." : "") + pieces[i].Replace(" ", "."):"");
                }
                else
                {
                    newhtml += pieces[i];
                }
            }
            return newhtml;
        }
        private string NonTurkhis(string text)
        {
            text = text.Replace("ı", "i");
            text = text.Replace("İ", "I");
            text = text.Replace("ö", "o");
            text = text.Replace("Ö", "O");
            text = text.Replace("ü", "u");
            text = text.Replace("Ü", "U");
            text = text.Replace("ç", "c");
            text = text.Replace("Ç", "C");
            text = text.Replace("ğ", "g");
            text = text.Replace("Ğ", "G");
            text = text.Replace("ş", "s");
            text = text.Replace("Ş", "S");
            return text;
        }
        #endregion
        #endregion
    }
}
