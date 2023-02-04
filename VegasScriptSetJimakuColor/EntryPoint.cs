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

            SettingForm dialog = new SettingForm()
            {
                TargetVideoTrackDataSource = trackNames,
                TargetVideoTrack = selected != null ? helper.GetTrackKey(selected) : trackNames[0],
                JimakuColor = VegasScriptSettings.JimakuColor,
                OutlineColor = VegasScriptSettings.OutlineColor,
                OutlineWidth = VegasScriptSettings.JimakuOutlineWidth,
            };

            if(dialog.ShowDialog() == DialogResult.Cancel) { return; }

            try
            {
                helper.SetTextParameterInTrack(
                    trackDict[dialog.TargetVideoTrack],
                    dialog.JimakuColor,
                    dialog.OutlineColor,
                    dialog.OutlineWidth
                    );

            }
            catch(VegasHelperNoneEventsException)
            {
                MessageBox.Show("選択したトラックにイベントがありません。");
            }

            VegasScriptSettings.JimakuColor = dialog.JimakuColor;
            VegasScriptSettings.OutlineColor = dialog.OutlineColor;
            VegasScriptSettings.JimakuOutlineWidth = dialog.OutlineWidth;
            VegasScriptSettings.Save();
        }
    }
}
