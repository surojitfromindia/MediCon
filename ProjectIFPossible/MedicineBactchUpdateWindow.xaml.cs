using ProjectIFPossible.ConnectionRouter;
using ProjectIFPossible.ConnectionRouter.MySqlClasses;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System;
namespace ProjectIFPossible
{
    /// <summary>
    /// Interaction logic for justCheck.xaml
    /// </summary>
    public partial class MedicineBatchEntry : Window
    {
        //Rules: 
        //1. All varible name must have  'v' (small v) as first letter.


        int vOpenFiled = 0;
        int vValidField = 0;
        int vEmptyField = 0;
        int vNotValidField = 0;

        /*
         * =================================Color Code: For validity=======================================
         * 
         * EMPTY_FIELD_COLOR            :- if field is empty
         * FIELD_IS_OK                  :- if field is not empty and medicine name is valid. wating for save
         * FIELD_IS_NOT_OK              :- if the medicine name is not valid
         * FIELD_OK_BUT_NOWHOLD_ZERO    :- if the medicine name is valid but new hold is empty or tha value is zero.
         * FIELD_IS_SUSPENDED           :- when all fields are suspended (Update Button is disabled).
         */
        private readonly SolidColorBrush EMPTY_FIELD_COLOR = new SolidColorBrush(Color.FromRgb(172,172,172));
        private readonly SolidColorBrush FIELD_IS_OK = new SolidColorBrush(Color.FromRgb(90, 249, 134));
        private readonly SolidColorBrush FIELD_OK_BUT_NOWHOLD_ZERO = new SolidColorBrush(Color.FromRgb(237, 247, 27));
        private readonly SolidColorBrush FIELD_IS_NOT_OK = new SolidColorBrush(Color.FromRgb(255, 64, 64));
        private readonly SolidColorBrush FIELD_IS_SUSPENDED = new SolidColorBrush(Colors.Coral);



        private bool vISsafeModeOn = true; //temp
        private HashSet<MedicineBatchUpdateControl> vControlHashSet = new HashSet<MedicineBatchUpdateControl>(50);
        private StackPanel ControlsHolderPanel;
        private string GLOBAL_USER = mySqlConnection.globalUser;
        private List<string> vItemSourceList;


        Dictionary<string, int> vDuplicateRecord = new Dictionary<string, int>();

        public MedicineBatchEntry()
        {
            InitializeComponent();
            ControlsHolderPanel = ParentPanel;
            vItemSourceList = MySqlUtil.MEDICINE_NAMES_LIST();

        }

        private void N_Click(object sender, RoutedEventArgs e)
        {
            MedicineBatchUpdateControl newMedicineUpdateControl = new MedicineBatchUpdateControl();
            RegisterName(newMedicineUpdateControl.Name, newMedicineUpdateControl); // Register this control on this window by Name (!important)                      
            AttachFields(newMedicineUpdateControl); //Attcah fields
            AttachEvents(newMedicineUpdateControl); //Attach events            
            ControlsHolderPanel.Children.Add(newMedicineUpdateControl);//add control to the parent panel (a stackPan)
            newMedicineUpdateControl.HorizontalAlignment = HorizontalAlignment.Left;
            vControlHashSet.Add(newMedicineUpdateControl);//add this control to the hashset;
            UpdateAllCounters();
        }



        private void RemoveItemsFromList(string eleName)
        {
            MedicineBatchUpdateControl childControl = (MedicineBatchUpdateControl)ControlsHolderPanel.FindName(eleName);
            if (vControlHashSet.Contains(childControl) != false)
            {
                vControlHashSet.Remove(childControl);
                ControlsHolderPanel.Children.Remove(childControl);
                UnregisterName(eleName);
                UpdateAllCounters();
            }
        }

        private void G_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("======");
            foreach (MedicineBatchUpdateControl j in vControlHashSet)
            {
                Console.WriteLine("{0}            {1}        {2}      {3}", j.medName, j.entryDate, j.manfName, j.expDate);
            }
        }


        //This will launch a window for confirmation pop-up.
        private void C_Click(object sender, RoutedEventArgs e)
        {
            MedicineSaveConfirmationWindow MCW = new MedicineSaveConfirmationWindow();
            MCW.SetHashSet(vControlHashSet);
            MCW.userName = GLOBAL_USER;
            MCW.ShowDialog();
        }


        private void SaveItemCurrentlyOnControl(string eleName)
        {
            MedicineBatchUpdateControl childControl = (MedicineBatchUpdateControl)ControlsHolderPanel.FindName(eleName);
            MySqlMedicineBatchEntry msmbe = new MySqlMedicineBatchEntry(childControl.medName, int.Parse(childControl.nowHold), childControl.price);
            Console.WriteLine(childControl.price);

            try
            {
                bool isOk = msmbe.SaveAndCommit(); //caution this can throw exception

                if (isOk)
                {
                    childControl.IsSaveButtonEanble = false;
                    //ControlsHolderPanel.Children.Remove(childControl);
                    MessageBox.Show("Medcine Updated!", "Medicine Update Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                    lblSaved.Text = (Convert.ToInt16(lblSaved.Text) + 1).ToString();
                }
            }
            catch (NonRegisterNameException e)
            {
                MessageBox.Show(e.StackTrace);
            }
        }




        /*
         * THIS FUCNTION WILL ASSIGN DEFAULT VALUES, AND EVENT HANDELERS AT THE
         * TIME OF CREATION
         */
        private void AttachEvents(MedicineBatchUpdateControl md)
        {
            md.thisControlRemoveEvent = RemoveItemsFromList;
            md.thisControlSaveButtonEvent = SaveItemCurrentlyOnControl;
            md.thisComboBoxItemChangeEvent = CheckValidityAndChangeAccordingly;
        }
        private void AttachFields(MedicineBatchUpdateControl md)
        {
            //this will be deafult date. don't change it.
            md.entryDate = DateTime.Now.Date.ToShortDateString();
            if (vISsafeModeOn)
                md.validColor = EMPTY_FIELD_COLOR;
            else
                md.validColor = FIELD_IS_SUSPENDED;
            md.ItemSoureceForComboBox = vItemSourceList;


        }





        /*THIS FUNCTION WILL TRIGGER EVERYTIME TEXT OF COMBOX IS CHNAGED*/
        private void CheckValidityAndChangeAccordingly(string eleName)
        {

            MedicineBatchUpdateControl childControl = (MedicineBatchUpdateControl)ControlsHolderPanel.FindName(eleName);
            MedicineDetails medDClass = new MedicineDetails(childControl.medName);
            validityIndicationState(childControl);
            childControl.cuurtBatc = medDClass.MED_CUR_BATCH();
            childControl.onHold = medDClass.MED_CUR_HOLD();
            childControl.expDate = DateTime.Now.Date.AddMonths(medDClass.MED_EXP_MONTH_DURATION());
            childControl.manfName = medDClass.MED_MANUF_NAME();
            childControl.price = medDClass.MED_CUR_PRICE();
            UpdateAllCounters();

        }
        private void validityIndicationState(MedicineBatchUpdateControl controlName)
        {

            if (vISsafeModeOn)
            {
                string ts = controlName.medName == null ? "" : controlName.medName;
                if (ts.Length == 0)
                    controlName.validColor = EMPTY_FIELD_COLOR;
                else if (vItemSourceList.Contains(ts))
                {
                    if (controlName.nowHold != "0")
                        controlName.validColor = FIELD_IS_OK;
                    else
                        controlName.validColor = FIELD_OK_BUT_NOWHOLD_ZERO;
                }
                else if (!vItemSourceList.Contains(ts))
                    controlName.validColor = FIELD_IS_NOT_OK;
            }

        }

        private void SafeToggleClick(object sender, RoutedEventArgs e)
        {
            if (vISsafeModeOn == true)
            {
                foreach (MedicineBatchUpdateControl ctr in vControlHashSet)
                {
                    ctr.IsSaveButtonEanble = false;
                    ctr.validColor = FIELD_IS_SUSPENDED;
                }
                vISsafeModeOn = false;
                btnSafeModeToogle.Background = new SolidColorBrush(Colors.RoyalBlue);
                btnSafeModeToogle.Content = "Safe Mode On";
            }
            else
            {
                vISsafeModeOn = true;
                foreach (MedicineBatchUpdateControl ctr in vControlHashSet)
                {
                    ctr.IsSaveButtonEanble = true;
                    validityIndicationState(ctr);
                }
                btnSafeModeToogle.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFB4E71"));
                btnSafeModeToogle.Content = "Safe Mode Off";
            }
        }
        private void DeleAllFiedlClick(object sender, RoutedEventArgs e)
        {
            DeleteAllField();
        }
        private void btnMergeDuplicate_Click(object sender, RoutedEventArgs e)
        {

            MergeDuplicate();
            DeleteAllField();
            foreach (var newEntry in vDuplicateRecord)
            {
                MedicineBatchUpdateControl newMedicineUpdateControl = new MedicineBatchUpdateControl();
                RegisterName(newMedicineUpdateControl.Name, newMedicineUpdateControl); // Register this control on this window by Name (!important)                      
                AttachFields(newMedicineUpdateControl); //Attcah fields
                AttachEvents(newMedicineUpdateControl); //Attach events
                newMedicineUpdateControl.medName = newEntry.Key;
                newMedicineUpdateControl.nowHold = newEntry.Value.ToString();
                ControlsHolderPanel.Children.Add(newMedicineUpdateControl);//add control to the parent panel (a stackPan)
                newMedicineUpdateControl.HorizontalAlignment = HorizontalAlignment.Left;
                vControlHashSet.Add(newMedicineUpdateControl);//add this control to the hashset;
            }
            UpdateAllCounters();
        }
        private void btnCloseThis_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void btnMaxThis_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
        }




        /*UPDATE COUNTERS FUNCTION GROUP (MEMORY HEAVY I GUESS)*/
        private void UpdateAllCounters()
        {
            UpdateOpenFieldCounter();
            UpdateEmptyFieldCounter();
            UpdateValidFieldCounter();
            UpdateInvalidFieldCounter();
        }
        private void UpdateOpenFieldCounter()
        {
            vOpenFiled = vControlHashSet.Count == 0 ? 0 : vControlHashSet.Count;
            lblFieldOpen.Text = vOpenFiled.ToString();
        }
        private void UpdateEmptyFieldCounter()
        {
            int j = 0;
            foreach (var e in vControlHashSet)
            {
                string ts = e.medName == null ? "" : e.medName;
                if (ts.Length == 0)
                {
                    j++;
                }
            }
            vEmptyField = j;
            

        }
        private void UpdateValidFieldCounter()
        {
            int j = 0;
            foreach (var e in vControlHashSet)
            {
                string ts = e.medName == null ? "" : e.medName;
                if (vItemSourceList.Contains(ts))
                {
                    j++;
                }
            }
            vValidField = j;
            lblValidMedi.Text = vValidField.ToString();
        }
        private void UpdateInvalidFieldCounter()
        {
            int c = vOpenFiled - vEmptyField - vValidField;
            vNotValidField = c;
            lblNotValidMedCount.Text = vNotValidField.ToString();
        }


        /*MERGE DUPLICATE*/
        private void MergeDuplicate()
        {
            vDuplicateRecord.Clear();
            foreach (var cn in vControlHashSet)
            {
                if (cn.medName.Length != 0 && vItemSourceList.Contains(cn.medName))
                {
                    if (!vDuplicateRecord.ContainsKey(cn.medName))
                    {
                        vDuplicateRecord[cn.medName] = int.Parse(cn.nowHold);
                    }
                    else
                        vDuplicateRecord[cn.medName] += int.Parse(cn.nowHold);
                }
            }


        }

        /*DELETE ALL FIEL*/
        private void DeleteAllField()
        {
            foreach (MedicineBatchUpdateControl childControl in vControlHashSet)
            {
                ControlsHolderPanel.Children.Remove(childControl);
                UnregisterName(childControl.Name);
            }
            vControlHashSet.Clear();
            UpdateAllCounters();

        }

        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btnBack_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Close();
        }

        public static readonly DependencyProperty ControlThemeProperty = DependencyProperty.Register("ControlTheme", typeof(Theme), typeof(MedicineBatchEntry));

        public Theme ControlTheme
        {
            set { SetValue(ControlThemeProperty, value); SetTheme(value); }
        }

        private void SetTheme(Theme value)
        {
            refWindow.Background = value.MainBodyBackColor;
            n.Background = value.ContainerBackColor;
        }
    }
}
