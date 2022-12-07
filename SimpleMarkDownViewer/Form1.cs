using Markdig;
using Markdig.Parsers;
using Markdig.Renderers;
using Microsoft.Web.WebView2.Core;
using System;
using System.Diagnostics;
using System.Security.Cryptography;

namespace SimpleMarkDownViewer
{
    public partial class Form1 : Form
    {
        private readonly MarkdownPipeline pipeline;

        public Form1()
        {
            InitializeComponent();
            pipeline = createPipeline();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private async void Form1_Shown(object sender, EventArgs e)
        {
            await webView21.EnsureCoreWebView2Async();

            var mdfiles = Directory.GetFiles(AppContext.BaseDirectory, "*.md");
            if (mdfiles.Count() == 0)
            {
                //do nothing
            }
            else if (mdfiles.Count() == 1)
            {
                //open the markdown file
                showMdFile(mdfiles[0]);
            }
            else
            {
                using (var ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Markdown files (*.md)|*.md";
                    ofd.InitialDirectory = AppContext.BaseDirectory;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        showMdFile(ofd.FileName);
                    }
                }
            }
        }

        private static MarkdownPipeline createPipeline()
        {
            var pipeline = new MarkdownPipelineBuilder()
                .UseYamlFrontMatter().Build();
            return pipeline;
        }

        private string render(string text)
        {
            var sw = new StringWriter();
            var render = new HtmlRenderer(sw);
            pipeline.Setup(render);

            var doc = MarkdownParser.Parse(text, pipeline);

            render.Render(doc);
            sw.Flush();
            return sw.ToString();
        }

        private void showMdFile(string filepath)
        {
            this.Text = "Simple Markdown Viewer - " + filepath;
            webView21.NavigateToString(render(File.ReadAllText(filepath)));
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Markdown files (*.md)|*.md";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    showMdFile(ofd.FileName);
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}