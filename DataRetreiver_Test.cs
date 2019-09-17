using System;
using System.Drawing;
using System.Data;

namespace CSGitCack
{
    public class DataRetreiver_Test
    {
        public static void retreiveData(System.Windows.Forms.DataGridView pobUnAcknowledgedGridView, System.Windows.Forms.DataGridView pobOnSiteGridView)
        {
            DataRetreiver dataretreiver = new DataRetreiver();
            int gateID = 82;

            // populate grids
            dataretreiver.populateUnacknowledgedPersonnel(pobUnAcknowledgedGridView, gateID);
            dataretreiver.populateAcknowledgedPersonnel(pobOnSiteGridView, gateID);
            String tagId = "";
            DataTable allTags = dataretreiver.getFirstUndisplayedTag(gateID);
            // loop through grid
            if (allTags.Rows.Count != 0)
                foreach (DataRow tag in allTags.Rows)
                {
                    tagId = allTags.Rows[0][0].ToString();
                    // retrieve personnel details
                    DataTable displayedData = dataretreiver.retrieveDisplayPersonnel(tag["ubisensetagid"].ToString());
                    if (displayedData.Rows.Count != 0)
                    {
                        foreach (DataRow row in displayedData.Rows)
                        {
                            String dob = row["dob"].ToString();
                            dob = dob.Substring(0, dob.IndexOf(":") - 3);

                            if (row["photograph"].ToString().Length > 0 && row["photograph"].ToString() != null)
                            {
                                var personnelPicture_BackgroundImage = new Bitmap(Conversions.convertByteToImage(row["photograph"]));
                            }
                            else
                            {
                                var personnelPicture_BackgroundImage = new Bitmap(dataretreiver.getResourcesFile() + "/PicNotAvailable.png");
                            }

                            if (row["batterystatus"].ToString() != "OK")
                            {
                                var batteryStatusPictureBox_BackgroundImage = new Bitmap(dataretreiver.getResourcesFile() + "/BatteryEmpty.png");
                                var stopGoPictureBox_BackgroundImage = new Bitmap(dataretreiver.getResourcesFile() + "/stopSignOn.jpg");
                            }
                            else
                            {
                                var batteryStatusPictureBox_BackgroundImage = new Bitmap(dataretreiver.getResourcesFile() + "/BatteryFull.png");
                                var stopGoPictureBox_BackgroundImage = new Bitmap(dataretreiver.getResourcesFile() + "/goSignOn.jpg");
                            }
                        }
                    }
                    // check if tag is assigned to vehicle and diplay appropriate label
                    else if (displayedData.Rows.Count == 0)
                    {
                        DataTable vehicles = dataretreiver.getVehicleTag(tag["ubisensetagid"].ToString());
                        if (vehicles.Rows.Count != 0)
                        {
                            foreach (DataRow row in vehicles.Rows)
                            {
                                var regLabel_Text = row["registration"].ToString();
                                var descLabel_Text = row["details"].ToString();
                            }
                        }
                    }
                }

            // there is no queue - display front screen logo
            if (allTags.Rows.Count == 0)
            {
                tagId = "";
            }
        }
    }
}
