using ScriptPortal.Vegas;
using System.Drawing;
using System.Collections.Generic;
using VegasScriptHelper;
using System.Linq;
using System.Windows.Forms;

namespace VegasScriptSetJimakuColor
{
    public class EntryPoint: IEntryPoint
    {
        private SettingForm settingForm = null;
        public void FromVegas(Vegas vegas)
        {
            VegasScriptSettings.Load();
            VegasHelper helper = VegasHelper.Instance(vegas);

            if (!helper.AllVideoTracks.Any())
            {
                MessageBox.Show("ビデオトラックがありません");
                return;
            }

            Dictionary<string, VideoTrack> trackDict = helper.GetVideoKeyValuePairs();
            List<string> trackNames = trackDict.Keys.ToList();

            VideoTrack selected = helper.SelectedVideoTrack(false);

            if(settingForm == null) { settingForm = new SettingForm(); }

            settingForm.TargetVideoTrackDataSource = trackNames;
            settingForm.TargetVideoTrack = selected != null ? helper.GetTrackKey(selected) : trackNames[0];
            settingForm.JimakuColor = VegasScriptSettings.JimakuColor;
            settingForm.OutlineColor = VegasScriptSettings.OutlineColor;
            settingForm.OutlineWidth = VegasScriptSettings.JimakuOutlineWidth;

            if(settingForm.ShowDialog() == DialogResult.Cancel) { return; }

            try
            {
                helper.SetTextParameterInTrack(
                    trackDict[settingForm.TargetVideoTrack],
                    settingForm.JimakuColor,
                    settingForm.OutlineColor,
                    settingForm.OutlineWidth
                    );

            }
            catch(VegasHelperNoneEventsException)
            {
                MessageBox.Show("選択したトラックにイベントがありません。");
            }

            VegasScriptSettings.JimakuColor = settingForm.JimakuColor;
            VegasScriptSettings.OutlineColor = settingForm.OutlineColor;
            VegasScriptSettings.JimakuOutlineWidth = settingForm.OutlineWidth;
            VegasScriptSettings.Save();
        }
    }
}
