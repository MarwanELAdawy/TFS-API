using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.TeamFoundation.Server;
using System.Diagnostics;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TfsAPIDemoProcessTemplate
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private TfsTeamProjectCollection _tfs;
        private string _selectedTeamProject;

        private void btnConnect_Click(object sender, EventArgs e)
        {
            TeamProjectPicker tfsPP = new TeamProjectPicker(TeamProjectPickerMode.SingleProject, false);
            tfsPP.ShowDialog();
            this._tfs = tfsPP.SelectedTeamProjectCollection;
            this._selectedTeamProject = tfsPP.SelectedProjects[0].Name;

            var processTemplateDetails = GetProcessTemplateDetailsForTheSelectedProject();

            listBox1.Items.Clear();
            listBox1.Items.Add(String.Format("Team Project Selected => '{0}'", _selectedTeamProject));
            listBox1.Items.Add(Environment.NewLine);
            var processTemplateName = processTemplateDetails.Where(pt => pt.Name == "Process Template").Select(pt => pt.Value).FirstOrDefault();

            if (!string.IsNullOrEmpty(processTemplateName))
            {
                listBox1.Items.Add(Environment.NewLine);
                listBox1.Items.Add(String.Format("Process Template Name: {0}", processTemplateName));
            }
            else
            {
                listBox1.Items.Add(String.Format("The Process Template does not have the 'Name' property set up"));
                listBox1.Items.Add(String.Format("***TIP: Download the Process Template and in Classification.xml add a new property Name, update the template then you will be able to see the Process Template Name***"));
                listBox1.Items.Add(String.Format(" - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -"));
            }
        }

        private ProjectProperty[] GetProcessTemplateDetailsForTheSelectedProject()
        {
            var vcs = _tfs.GetService<VersionControlServer>();
            var ics = _tfs.GetService<ICommonStructureService>();
            var pt = _tfs.GetService<IProcessTemplates>();
            ProjectProperty[] ProjectProperties = null;

            var p = vcs.GetTeamProject(_selectedTeamProject);
            string ProjectName = string.Empty;
            string ProjectState = String.Empty;
            int templateId = 0;
            ProjectProperties = null;
            
            ics.GetProjectProperties(p.ArtifactUri.AbsoluteUri, out ProjectName, out ProjectState, out templateId, out ProjectProperties);

            return ProjectProperties;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
