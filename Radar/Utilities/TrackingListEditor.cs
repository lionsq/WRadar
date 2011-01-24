﻿using System;
using System.Linq;
using System.Windows.Forms;
using Radar.Tracking;
using RadarSettings = Radar.Settings;

namespace Radar.Utilities
{
    public partial class TrackingListEditor : Form
    {
        #region Properties

        internal TrackingList SelectedList { get; private set; }

        #endregion

        public TrackingListEditor()
        {
            InitializeComponent();

            SelectedList = RadarSettings.Tracking.Default;
            _listsCombo.DataSource = RadarSettings.Tracking.TrackingLists;
            _listsCombo.DisplayMember = "Name";
            _listsCombo.SelectedItem = SelectedList;

            _trackablesLstBx.DataSource = SelectedList;
            _trackablesLstBx.DisplayMember = "Name";
            _trackablesLstBx.Refresh();
        }

        private void NewTrackable(object sender, EventArgs e)
        {
            var track = new Trackable();
            var te = new TrackableEditor(track);

            te.ShowDialog();

            if (string.IsNullOrEmpty(track.Pattern) || string.IsNullOrEmpty(track.Name))
            {
                return;
            }

            SelectedList.Add(track);

            _trackablesLstBx.DataSource = null;
            _trackablesLstBx.DataSource = SelectedList;
            _trackablesLstBx.DisplayMember = "Name";
            _trackablesLstBx.Refresh();
        }

        private void EditTrackable(object sender, EventArgs e)
        {
            if (_trackablesLstBx.SelectedIndex < 0 || _trackablesLstBx.SelectedItems.Count > 1)
                return;

            var track = _trackablesLstBx.SelectedItem as Trackable;
            var te = new TrackableEditor(track);

            te.ShowDialog();
            _trackablesLstBx.DataSource = null;
            _trackablesLstBx.DataSource = SelectedList;
            _trackablesLstBx.DisplayMember = "Name";
            _trackablesLstBx.Refresh();
        }

        private void DeleteTrackables(object sender, EventArgs e)
        {
            foreach (var trackable in
                from object selectedItem in _trackablesLstBx.SelectedItems select selectedItem as Trackable)
            {
                SelectedList.Remove(trackable);
            }

            _trackablesLstBx.DataSource = null;
            _trackablesLstBx.DataSource = SelectedList;
            _trackablesLstBx.DisplayMember = "Name";
            _trackablesLstBx.Refresh();
        }

        private void ListChanged(object sender, EventArgs e)
        {
            SelectedList = _listsCombo.SelectedItem as TrackingList;

            _trackablesLstBx.Items.Clear();
            _trackablesLstBx.DataSource = null;
            _trackablesLstBx.DataSource = SelectedList;
            _trackablesLstBx.DisplayMember = "Name";
            _trackablesLstBx.Refresh();
        }

        private void ReloadInfo(object sender, EventArgs e)
        {
            _trackablesLstBx.DataSource = null;
            _trackablesLstBx.DataSource = SelectedList;
            _trackablesLstBx.DisplayMember = "Name";
            _trackablesLstBx.Refresh();
        }
    }
}