Imports System.Windows.Forms
Imports System.IO
Imports System.Configuration
Imports Utilities
Imports System.Data.SqlClient
Imports System.Drawing

Public Class Main


    Public Sub New()
        ' Get the application's application domain.
        Dim currentDomain As AppDomain = AppDomain.CurrentDomain

        ' Define a handler for unhandled exceptions.
        AddHandler currentDomain.UnhandledException, AddressOf MYExnHandler

        ' Define a handler for unhandled exceptions for threads behind forms.
        AddHandler Application.ThreadException, AddressOf MYThreadHandler

        InitializeComponent()
    End Sub


    ' Application unhandled exception handler (catch all)
    Private Sub MYExnHandler(ByVal sender As Object, ByVal e As UnhandledExceptionEventArgs)
        Dim EX As Exception
        EX = e.ExceptionObject

        Log(DateTime.Now.ToString())
        Log("Unhandled Exception Event")
        Log(EX.Message)
        Log(EX.StackTrace)
        If (EX.InnerException IsNot Nothing) Then
            Log(EX.InnerException.Message)
            Log(EX.InnerException.StackTrace)
        End If
    End Sub


    ' Application Thread unhandled exception handler (catch all)
    Private Sub MYThreadHandler(ByVal sender As Object, ByVal e As Threading.ThreadExceptionEventArgs)
        Dim EX As Exception
        EX = e.Exception

        Log(DateTime.Now.ToString())
        Log("Thread Exception Event")
        Log(EX.Message)
        Log(EX.StackTrace)
        If (EX.InnerException IsNot Nothing) Then
            Log(EX.InnerException.Message)
            Log(EX.InnerException.StackTrace)
        End If
    End Sub


    ' Write log msg string to file
    Public Sub Log(strToWrite As String)
        Dim s = Date.Now.ToString() & " " & strToWrite
        Using writer As StreamWriter = New StreamWriter("c:\DeleteMe\PODFOMain.Critical.Log.txt", True)
            writer.WriteLine(strToWrite)
        End Using
    End Sub




    Private Sub ShowNewForm(ByVal sender As Object, ByVal e As EventArgs)
        ' Create a new instance of the child form.
        Dim ChildForm As New System.Windows.Forms.Form
        ' Make it a child of this MDI form before showing it.
        ChildForm.MdiParent = Me

        m_ChildFormNumber += 1
        ChildForm.Text = "Window " & m_ChildFormNumber

        ChildForm.Show()
    End Sub


    Private Sub ExitToolsStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
        Me.Close()
    End Sub


    Private Sub CascadeToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
        Me.LayoutMdi(MdiLayout.Cascade)
    End Sub


    Private Sub TileVerticalToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
        Me.LayoutMdi(MdiLayout.TileVertical)
    End Sub


    Private Sub TileHorizontalToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
        Me.LayoutMdi(MdiLayout.TileHorizontal)
    End Sub


    Private Sub ArrangeIconsToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
        Me.LayoutMdi(MdiLayout.ArrangeIcons)
    End Sub


    Private Sub CloseAllToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
        ' Close all child forms of the parent.
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next
    End Sub




    Private m_ChildFormNumber As Integer


    Private Sub FullToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles FullToolStripMenuItem.Click
        Dim frmChildForm As Form

        For Each frmChildForm In Me.MdiChildren

            If frmChildForm.Text = "ReRunFull" Then
                frmChildForm.WindowState = FormWindowState.Maximized
                Exit Sub
            End If
        Next

        Dim frmChild As New ReRunFull
        frmChild.MdiParent = Me
        frmChild.WindowState = FormWindowState.Maximized
        frmChild.Show()
    End Sub


    Private Sub SelectRangeToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles SelectRangeToolStripMenuItem.Click
        Dim frmChildForm As Form

        For Each frmChildForm In Me.MdiChildren

            If frmChildForm.Text = "ReRunRange" Then
                frmChildForm.WindowState = FormWindowState.Maximized
                Exit Sub
            End If
        Next

        Dim frmChild As New ReRunRange
        frmChild.MdiParent = Me
        frmChild.WindowState = FormWindowState.Maximized
        frmChild.Show()
    End Sub


    Private Sub SelectLetterToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles SelectLetterToolStripMenuItem.Click
        Dim frmChildForm As Form

        For Each frmChildForm In Me.MdiChildren

            If frmChildForm.Text = "ReRunLetter" Then
                frmChildForm.WindowState = FormWindowState.Maximized
                Exit Sub
            End If
        Next

        Dim frmChild As New ReRunLetter
        frmChild.MdiParent = Me
        frmChild.WindowState = FormWindowState.Maximized
        frmChild.Show()
    End Sub


    Public Sub SetText(ByVal text As String)
        ToolStripStatusLabel.Text = text
    End Sub


    Private Sub cbProdTest_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cbProdTest.SelectedIndexChanged
        Dim s As String
        s = If(cbProdTest.Text = "PROD", "False", "True")
        ConfigurationManager.AppSettings.Set("UseTestDB", s)

        ' My.Settings.Default("PODFOConnectionString") = DbAccess.GetConnectionString()
        ' s = My.MySettings.Default("PODFOConnectionString")

        DbAccess.Close()
        If (cbProdTest.Text <> "PROD") Then
            Me.SampleCreatorToolStripMenuItem.Visible = True
        Else
            Me.SampleCreatorToolStripMenuItem.Visible = False
        End If
    End Sub


    Private Sub Main_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

        ' Show Version Number and EXE File date in Title Bar
        Dim Ver As String, BDate As String
        Ver = My.Application.Info.Version.ToString
        BDate = File.GetLastWriteTime(Reflection.Assembly.GetExecutingAssembly.Location).ToString()
        Me.Text = String.Format("PODFO - Version:{0}   File Date: {1}", Ver, BDate)

        ' Support for UI specified Use Test Server option
        Dim useTestDb As Boolean = False
        Dim useTestDatabaseAppSetting As String = ConfigurationManager.AppSettings("UseTestDB")
        If Not String.IsNullOrEmpty(useTestDatabaseAppSetting) Then
            If Not Boolean.TryParse(useTestDatabaseAppSetting, useTestDb) Then
                useTestDb = False
            End If
        End If

        ' Setup Prod / Test combo box
        Me.cbProdTest.Items.Clear()
        Me.cbProdTest.Items.Add("PROD")
        Me.cbProdTest.Items.Add("TEST")
        Me.cbProdTest.Text = If(useTestDb, "TEST", "PROD")
        If (useTestDb = True) Then
            Me.SampleCreatorToolStripMenuItem.Visible = True
        Else
            Me.SampleCreatorToolStripMenuItem.Visible = False
        End If

        'MessageBox.Show("Make sure you update file date before release in Decemeber 2015")
    End Sub


    ' Close connections on closing
    Private Sub Main_Closing(sender As System.Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        DbAccess.Close()
    End Sub

    Private Sub SampleCreatorToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles SampleCreatorToolStripMenuItem.Click
        Dim useTestDb As Boolean = False
        Dim Conn As String = Nothing
        Dim conf As ConfigTable = Nothing
        Dim log As Logging = Nothing

        Conn = DbAccess.GetConnectionString()
        useTestDb = DbAccess.UseTestDB


        conf = New ConfigTable(Conn)
        conf.DefaultGroupName = If((useTestDb), "PODFOReports.Test", "PODFOReports")

        If (useTestDb = True) Then

            Dim Names As String() = New String() {}
            Dim Addresses As String() = New String() {}
            Dim Cities As String() = New String() {}
            Dim States As String() = New String() {}
            Dim Zips As String() = New String() {}

            Names = {"Marinda Mastropietro",
    "Ayako Atkin",
    "Gladys Grogan",
    "Erlinda Ewald",
    "Eugene Ericson",
    "Many Marrone",
    "Jacquie Jakes",
    "Neoma Nalley",
    "Lavon Legleiter",
    "Shera Schoening",
    "Tiana Tassin",
    "Natalya Norberg",
    "Erma Ebersole",
    "Lang Lezama",
    "Luna Luby",
    "Ardis Ashurst",
    "Brandon Biggers",
    "Angelyn Aarons",
    "Claudine Cimmino",
    "Eldon Ericksen",
    "Herbert Hobdy",
    "Brenna Beaubien",
    "Toshiko Tutson",
    "Stephanie Shor",
    "Marc Maselli",
    "Kasha Kull",
    "Luvenia Lawley",
    "Jeana Jeter",
    "Shaniqua Serafino",
    "Shonta Shaeffer",
    "Keitha Keep",
    "Sherlene Spivey",
    "Helena Hammersmith",
    "Chasidy Cofield",
    "Cecil Casali",
    "Tish Tyra",
    "Herlinda Hara",
    "Brigette Benner",
    "Amy Amburgey",
    "Lon Lindauer",
    "Donn Deason",
    "Shelli Sauceda",
    "Ardith Arnwine",
    "Celina Coghill",
    "Un Uchida",
    "Keneth Kuhns",
    "Santa Skelton",
    "Carson Clay",
    "Otis Odegaard",
    "Elise Eldridge"}

            Addresses = {"6649 N Blue Gum St",
    "4 B Blue Ridge Blvd",
    "8 W Cerritos Ave #54",
    "639 Main St",
    "34 Center St",
    "3 Mcauley Dr",
    "7 Eads St",
    "7 W Jackson Blvd",
    "5 Boston Ave #88",
    "228 Runamuck Pl #2808",
    "2371 Jerrold Ave",
    "37275 St  Rt 17m M",
    "25 E 75th St #69",
    "98 Connecticut Ave Nw",
    "56 E Morehead St",
    "73 State Road 434 E",
    "69734 E Carrillo St",
    "322 New Horizon Blvd",
    "1 State Route 27",
    "394 Manchester Blvd",
    "6 S 33rd St",
    "6 Greenleaf Ave",
    "618 W Yakima Ave",
    "74 S Westgate St",
    "3273 State St",
    "1 Central Ave",
    "86 Nw 66th St #8673",
    "2 Cedar Ave #84",
    "90991 Thorburn Ave",
    "386 9th Ave N",
    "74874 Atlantic Ave",
    "366 South Dr",
    "45 E Liberty St",
    "4 Ralph Ct",
    "2742 Distribution Way",
    "426 Wolf St",
    "128 Bransten Rd",
    "17 Morena Blvd",
    "775 W 17th St",
    "6980 Dorsett Rd",
    "2881 Lewis Rd",
    "7219 Woodfield Rd",
    "1048 Main St",
    "678 3rd Ave",
    "20 S Babcock St",
    "2 Lighthouse Ave",
    "38938 Park Blvd",
    "5 Tomahawk Dr",
    "762 S Main St",
    "209 Decker Dr"}

            Zips = {"70116",
    "48116",
    "08014",
    "99501",
    "45011",
    "44805",
    "60632",
    "95111",
    "57105",
    "21224",
    "19443",
    "11953",
    "90034",
    "44023",
    "78045",
    "85013",
    "37110",
    "53207",
    "48180",
    "61109",
    "19014",
    "95111",
    "75062",
    "12204",
    "08846",
    "54481",
    "66218",
    "21601",
    "10011",
    "77301",
    "43215",
    "88011",
    "07660",
    "08812",
    "10025",
    "70002",
    "10011",
    "93012",
    "78204",
    "67410",
    "97754",
    "66204",
    "99708",
    "33196",
    "99712",
    "55343",
    "02128",
    "90006",
    "53711"}

            States = {"LA",
    "MI",
    "NJ",
    "AK",
    "OH",
    "OH",
    "IL",
    "CA",
    "SD",
    "MD",
    "PA",
    "NY",
    "CA",
    "OH",
    "TX",
    "AZ",
    "TN",
    "WI",
    "MI",
    "IL",
    "PA",
    "CA",
    "TX",
    "NY",
    "NJ",
    "WI",
    "KS",
    "MD",
    "NY",
    "TX",
    "OH",
    "NM",
    "NJ",
    "NJ",
    "NY",
    "LA",
    "NY",
    "CA",
    "TX",
    "KS",
    "OR",
    "KS",
    "AK",
    "FL",
    "AK",
    "MN",
    "MA",
    "CA",
    "WI",
    "PA"}
            Cities = {"New Orleans",
    "Brighton",
    "Bridgeport",
    "Anchorage",
    "Hamilton",
    "Ashland",
    "Chicago",
    "San Jose",
    "Sioux Falls",
    "Baltimore",
    "Kulpsville",
    "Middle Island",
    "Los Angeles",
    "Chagrin Falls",
    "Laredo",
    "Phoenix",
    "Mc Minnville",
    "Milwaukee",
    "Taylor",
    "Rockford",
    "Aston",
    "San Jose",
    "Irving",
    "Albany",
    "Middlesex",
    "Stevens Point",
    "Shawnee",
    "Easton",
    "New York",
    "Conroe",
    "Columbus",
    "Las Cruces",
    "Ridgefield Park",
    "Dunellen",
    "New York",
    "Metairie",
    "New York",
    "Camarillo",
    "San Antonio",
    "Abilene",
    "Prineville",
    "Overland Park",
    "Fairbanks",
    "Miami",
    "Fairbanks",
    "Hopkins",
    "Boston",
    "Los Angeles",
    "Madison",
    "Philadelphia"}

            Dim cmdSQL As New SqlCommand
            Dim parSQL As SqlParameter
            Dim rdSQL As SqlDataAdapter
            Dim ds As DataSet = New DataSet()
            Dim strHCIN As String = String.Empty
            Dim intImportFile = -1
            Dim strMasterLetterTypeID As String = String.Empty
            Dim strMEndore As String = "***************AUTO**MIXED AADC 720               "
            Dim MBarcode As String = "AFAFAADTATTDAAADTATTATDTFDFTFTTFATATTATDDDFATTDAFTAATTFADTATATFTT"
            Dim MBarcodeNumber As String = "5026090438700182743135215730916"
            Dim uniqueID As String = String.Empty
            Dim mpresortID As Integer = 0
            Dim MKeyline As String = "1405 1 MB 0.435"
            Dim strLetterType As String = String.Empty
            cmdSQL.CommandTimeout = 0
            cmdSQL.Connection = DbAccess.GetConnection()
            cmdSQL.CommandText = "select * from PODMasterLetterType where LetterType <> 'WC ' AND LetterType <> 'BAD' and PODMasterLetterTypeid not in (5,6,13,14,27,28,38)"
            cmdSQL.CommandType = CommandType.Text

            rdSQL = New SqlDataAdapter(cmdSQL)

            rdSQL.Fill(ds)
            rdSQL.Dispose()
            cmdSQL.Dispose()

            cmdSQL = New SqlCommand

            cmdSQL.CommandText = "USP_INSERT_MAILINFO_FAKE_DATA"
            cmdSQL.CommandType = CommandType.StoredProcedure
            cmdSQL.CommandTimeout = 0
            cmdSQL.Connection = DbAccess.GetConnection()
            Dim count As Int32 = ds.Tables(0).Rows.Count

            For Each dr As DataRow In ds.Tables(0).Rows

                cmdSQL.Parameters.Clear()
                mpresortID += 1
                strMasterLetterTypeID = dr("PODMasterLetterTypeId").ToString()
                strLetterType = dr("LetterType").ToString()
                strHCIN = GenerateRandomString(10, False)
                uniqueID = GenerateRandomString(9, False)

                cmdSQL.Parameters.AddWithValue("@P_PODImportFilesID", 1)
                cmdSQL.Parameters.AddWithValue("@P_PODMasterLetterTypeId", Convert.ToInt16(strMasterLetterTypeID))
                cmdSQL.Parameters.AddWithValue("@P_LetterType", strLetterType)
                cmdSQL.Parameters.AddWithValue("@P_UniqueIdentifier", uniqueID)
                cmdSQL.Parameters.AddWithValue("@P_HICN", strHCIN)
                cmdSQL.Parameters.AddWithValue("@P_MAILNAME", Names(mpresortID))
                cmdSQL.Parameters.AddWithValue("@P_SatoriAdd1", Addresses(mpresortID))
                cmdSQL.Parameters.AddWithValue("@P_SatoriCityStateZip", Cities(mpresortID) + ", " + States(mpresortID) + " " + Zips(mpresortID))
                cmdSQL.Parameters.AddWithValue("@P_MBarcode", MBarcode)
                cmdSQL.Parameters.AddWithValue("@P_MBarcodeNumber", MBarcodeNumber)

                cmdSQL.Parameters.AddWithValue("@P_MEndorse", strMEndore)
                'cmdSQL.Parameters.AddWithValue("@P_PODBatchID", "")
                'cmdSQL.Parameters.AddWithValue("@P_Count", count)
                cmdSQL.Parameters.AddWithValue("@P_PODRunID", "01")
                cmdSQL.Parameters.AddWithValue("@P_MPresortID", mpresortID)
                cmdSQL.Parameters.AddWithValue("@P_TempKeyId", mpresortID)


                'aco start
                cmdSQL.Parameters.AddWithValue("@P_EffectiveDate", "19000102")
                cmdSQL.Parameters.AddWithValue("@P_LegalName", "USA IMAGES HEALTH - 123")
                cmdSQL.Parameters.AddWithValue("@P_DataSharing", "N")
                cmdSQL.Parameters.AddWithValue("@P_SharingEffectiveDate", "19000102")
                cmdSQL.Parameters.AddWithValue("@P_SharingPreferenceIndicator", "N")
                'aco end

                'DIS START
                cmdSQL.Parameters.AddWithValue("@P_CurrentDate", "02/01/1900")
                cmdSQL.Parameters.AddWithValue("@P_CallStartTime", "02/01/1900 12:00:00")
                'cmdSQL.Parameters.AddWithValue("@P_ActivityID", "")
                cmdSQL.Parameters.AddWithValue("@P_PlanName", "USA IMAGES HEALTH - 123")
                cmdSQL.Parameters.AddWithValue("@P_ActivityID", uniqueID)
                cmdSQL.Parameters.AddWithValue("@P_TeminationDate", "02/01/1900")
                cmdSQL.Parameters.AddWithValue("@P_CreateBy", GenerateRandomString(9, True))
                'DIS END

                'MBP START
                cmdSQL.Parameters.AddWithValue("@P_PassWord", GenerateRandomString(5, True))
                cmdSQL.Parameters.AddWithValue("@P_RegistrationDate", "02/01/1900 12:00:00")
                cmdSQL.Parameters.AddWithValue("@P_PatternedPassword", "")
                cmdSQL.Parameters.AddWithValue("@P_CSRRestPassword", "")
                cmdSQL.Parameters.AddWithValue("@P_AuthorizeRepRelationShip", "Beneficiary")
                cmdSQL.Parameters.AddWithValue("@P_AuthorizeRepName", "")
                'MBP END

                'ENT START
                cmdSQL.Parameters.AddWithValue("@P_PartAStartDate", "02/01/1900")
                cmdSQL.Parameters.AddWithValue("@P_PartBStartDate", "02/01/1900")
                cmdSQL.Parameters.AddWithValue("@P_SystemLetter", strLetterType)
                'ENT END

                'CPC START
                cmdSQL.Parameters.AddWithValue("@P_PracticeName", "USA IMAGES HEALTH - 123")
                'CPC END

                cmdSQL.ExecuteNonQuery()
            Next
            cmdSQL.Parameters.Clear()
            cmdSQL.Dispose()
            cmdSQL = Nothing


            cmdSQL = New SqlCommand

            cmdSQL.CommandText = "USP_INSERT_PodBatch_Fake_INFO"
            cmdSQL.CommandType = CommandType.StoredProcedure
            cmdSQL.CommandTimeout = 0
            cmdSQL.Connection = DbAccess.GetConnection()

            cmdSQL.Parameters.AddWithValue("@P_Count", count)
            cmdSQL.ExecuteNonQuery()

            cmdSQL.Dispose()
            cmdSQL = Nothing




        End If

    End Sub
    Public Function GenerateRandomString(ByRef len As Integer, ByRef upper As Boolean) As String
        Dim rand As New Random()
        Dim allowableChars() As Char = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLOMNOPQRSTUVWXYZ0123456789".ToCharArray()
        Dim final As String = String.Empty
        For i As Integer = 0 To len - 1
            final += allowableChars(rand.Next(allowableChars.Length - 1))
        Next

        Return IIf(upper, final.ToUpper(), final)
    End Function
End Class
