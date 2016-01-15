Imports System
Imports System.Management
Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Globalization

Public Class Form1

    Public ARKE_VNum = "1.2.0 d-0264"
    Public ARKE_VMin = "173.0"
    Public ARKE_VMax = "231.7"
    Public ARKE_VDate = "14/01/2016"
    Public ARKE_VLin = "0"
    Public ARKE_VLUT = "0"
    Public ARKE_VAUD = "false"

    Public nfi As NumberFormatInfo = New CultureInfo("en-US", False).NumberFormat
    'Thread.CurrentThread.CurrentCulture = New CultureInfo("en-US")
    Public localfolder As String = Application.StartupPath

    Public msini As New IniFile
    Public gsini As New IniFile
    Public bsini As New IniFile
    Public arkessini As New IniFile

    Public msfile As String
    Public gsfile As String
    Public bsfile As String
    Public arkess As String
    Public arkegpu As String
    Public arkebkp As String

    Public gpudata As New ArrayList


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Application.CurrentCulture = New CultureInfo("en-US")
        Me.Text = "ArkEnhancer v" + ARKE_VNum + " " + ARKE_VDate + " (For Ark " + ARKE_VMin + " - " + ARKE_VMax + ")"
        'MsgBox(localfolder & "\arkespecialsettings.ini")

        GetINIFiles()
    End Sub


    Public Sub GetINIFiles()
        FolderBrowserDialog1.SelectedPath = ""
        If File.Exists("C:\arkespecialsettings.ini") Or File.Exists(localfolder & "\arkespecialsettings.ini") Then
            If File.Exists(localfolder & "\arkespecialsettings.ini") Then
                arkessini.Load(localfolder & "\arkespecialsettings.ini")
            ElseIf File.Exists("C:\arkespecialsettings.ini") Then
                arkessini.Load("C:\arkespecialsettings.ini")
            End If
            arkess = arkessini.GetKeyValue("SystemSettings", "programfilesname")
            If (arkess = "") Then
                arkess = "C:\Program Files (x86)\Steam\SteamApps\common\ARK\ShooterGame"
            End If
            arkegpu = arkessini.GetKeyValue("SystemSettings", "selgpuindex")
            If (arkegpu = "") Then
                arkegpu = 0
            End If
            arkebkp = arkessini.GetKeyValue("AppSettings", "makebackup")
            If (arkebkp = "") Then
                arkebkp = True
            End If
            'MsgBox(arkebkp)
            CheckBox24.Checked = arkebkp

            ARKE_VLin = arkessini.GetKeyValue("AppSettings", "BuildLine")
            If ARKE_VLin = "" Then
                ARKE_VLin = "0"
            End If
            ARKE_VLUT = arkessini.GetKeyValue("AppSettings", "UpdateLast")
            If ARKE_VLUT = "" Then
                ARKE_VLUT = "Never"
            End If
            ARKE_VAUD = arkessini.GetKeyValue("AppSettings", "AutoUpdate")
            If ARKE_VAUD = "" Then
                ARKE_VAUD = "false"
            End If

            If File.Exists(arkess & "\SteamApps\common\ARK\ShooterGame\Saved\Config\WindowsNoEditor\Engine.ini") Then
                msfile = arkess & "\SteamApps\common\ARK\ShooterGame\Saved\Config\WindowsNoEditor\Engine.ini"
                gsfile = arkess & "\SteamApps\common\ARK\ShooterGame\Saved\Config\WindowsNoEditor\GameUserSettings.ini"
                bsfile = arkess & "\SteamApps\common\ARK\Engine\Config\BaseScalability.ini"
            ElseIf File.Exists(arkess & "\Saved\Config\WindowsNoEditor\Engine.ini") Then
                msfile = arkess & "\Saved\Config\WindowsNoEditor\Engine.ini"
                gsfile = arkess & "\Saved\Config\WindowsNoEditor\GameUserSettings.ini"
                bsfile = arkess.Substring(0, arkess.Length - 12) & "\Engine\Config\BaseScalability.ini"
            Else
                MsgBox("ARKE was unable to locate your settings files. Please select the directory ShooterGame resides in." & vbCrLf & "Example: C:\Program Files (x86)\Steam\SteamApps\common\ARK\ShooterGame", MsgBoxStyle.Exclamation)
                'FolderBrowserDialog1.ShowDialog()
                If DialogResult.OK = FolderBrowserDialog1.ShowDialog() Then
                    arkess = FolderBrowserDialog1.SelectedPath
                    msfile = arkess & "\Saved\Config\WindowsNoEditor\Engine.ini"
                    gsfile = arkess & "\Saved\Config\WindowsNoEditor\GameUserSettings.ini"
                    bsfile = arkess.Substring(0, arkess.Length - 12) & "\Engine\Config\BaseScalability.ini"
                    arkessini.SetKeyValue("SystemSettings", "programfilesname", arkess)
                    arkessini.Save(localfolder & "\arkespecialsettings.ini")
                Else
                    Application.Exit()
                End If
            End If
        Else
            If File.Exists("C:\Program Files (x86)\Steam\SteamApps\common\ARK\ShooterGame\Saved\Config\WindowsNoEditor\Engine.ini") Then
                msfile = "C:\Program Files (x86)\Steam\SteamApps\common\ARK\ShooterGame\Saved\Config\WindowsNoEditor\Engine.ini"
                gsfile = "C:\Program Files (x86)\Steam\SteamApps\common\ARK\ShooterGame\Saved\Config\WindowsNoEditor\GameUserSettings.ini"
                bsfile = "C:\Program Files (x86)\Steam\SteamApps\common\ARK\Engine\Config\BaseScalability.ini"
            ElseIf File.Exists("C:\Program Files\Steam\SteamApps\common\ARK\ShooterGame\Saved\Config\WindowsNoEditor\Engine.ini") Then
                msfile = "C:\Program Files\Steam\SteamApps\common\ARK\ShooterGame\Saved\Config\WindowsNoEditor\Engine.ini"
                gsfile = "C:\Program Files\Steam\SteamApps\common\ARK\ShooterGame\Saved\Config\WindowsNoEditor\GameUserSettings.ini"
                bsfile = "C:\Program Files\Steam\SteamApps\common\ARK\Engine\Config\BaseScalability.ini"
            Else
                MsgBox("ARKE was unable to locate your settings files. Please select the directory ShooterGame resides in." & vbCrLf & "Example: C:\Program Files (x86)\Steam\SteamApps\common\ARK\ShooterGame", MsgBoxStyle.Exclamation)
                'FolderBrowserDialog1.ShowDialog()
                If DialogResult.OK = FolderBrowserDialog1.ShowDialog() Then
                    arkess = FolderBrowserDialog1.SelectedPath
                    msfile = arkess & "\Saved\Config\WindowsNoEditor\Engine.ini"
                    gsfile = arkess & "\Saved\Config\WindowsNoEditor\GameUserSettings.ini"
                    bsfile = arkess.Substring(0, arkess.Length - 12) & "\Engine\Config\BaseScalability.ini"
                    arkessini.SetKeyValue("SystemSettings", "programfilesname", arkess)
                    arkessini.Save(localfolder & "\arkespecialsettings.ini")
                Else
                    Application.Exit()
                End If

            End If
        End If
        If Not File.Exists(msfile) And Not FolderBrowserDialog1.SelectedPath = "" Then
            GetINIFiles()
        End If
    End Sub


    Private Sub Form1_Shown(sender As Object, e As EventArgs) Handles Me.Shown

        If ARKE_VAUD Then
            CheckForUpdate()
        End If
        DoLoadINI()

    End Sub

    Sub DoLoadINI()
        Button1.Visible = False
        pb_sysinfo.Value = 0

        'l_gpu.Text = GetGraphicsCardName()
        GetCPUName()
        pb_sysinfo.Value = 1
        GetGraphicsCardName()
        pb_sysinfo.Value = 2
        'If File.Exists("C:\Program Files (x86)\Steam\SteamApps\common\ARK\ShooterGame\Saved\Config\WindowsNoEditor\Engine.ini") Then
        'msfile = "C:\Program Files (x86)\Steam\SteamApps\common\ARK\ShooterGame\Saved\Config\WindowsNoEditor\Engine.ini"
        'gsfile = "C:\Program Files (x86)\Steam\SteamApps\common\ARK\ShooterGame\Saved\Config\WindowsNoEditor\GameUserSettings.ini"
        'ElseIf File.Exists("C:\Program Files\Steam\SteamApps\common\ARK\ShooterGame\Saved\Config\WindowsNoEditor\Engine.ini") Then
        'msfile = "C:\Program Files\Steam\SteamApps\common\ARK\ShooterGame\Saved\Config\WindowsNoEditor\Engine.ini"
        'gsfile = "C:\Program Files\Steam\SteamApps\common\ARK\ShooterGame\Saved\Config\WindowsNoEditor\GameUserSettings.ini"
        'End If

        msini.Load(msfile)
        gsini.Load(gsfile)
        'MsgBox(bsfile)
        bsini.Load(bsfile)

        pb_sysinfo.Value = 3

        Dim c1 As String = msini.GetKeyValue("SystemSettings", "r.PostProcessAAQuality")
        If c1 IsNot "" Then
            CheckBox1.Checked = True
        End If
        'pb_sysinfo.Value += 1

        Dim c2 As String = msini.GetKeyValue("SystemSettings", "r.ShadowQuality")
        If c2 IsNot "" Then
            CheckBox2.Checked = True
        End If
        'pb_sysinfo.Value += 1

        Dim c3 As String = msini.GetKeyValue("SystemSettings", "r.MotionBlurQuality")
        If c3 IsNot "" Then
            CheckBox3.Checked = True
        End If
        'pb_sysinfo.Value += 1

        Dim c4 As String = msini.GetKeyValue("SystemSettings", "r.AmbientOcclusionLevels")
        If c4 IsNot "" Then
            CheckBox4.Checked = True
        End If
        'pb_sysinfo.Value += 1

        Dim c13 As String = msini.GetKeyValue("SystemSettings", "r.SSR")
        If c13 IsNot "" Then
            CheckBox13.Checked = True
        End If
        'pb_sysinfo.Value += 1

        'Dim c5 As String = msini.GetKeyValue("SystemSettings", "r.BloomQuality")
        Dim c5 As String = bsini.GetKeyValue("PostProcessQuality@0", "r.BloomQuality")
        If c5 = "0" Then
            CheckBox5.Checked = True
        End If
        'pb_sysinfo.Value += 1

        Dim c6 As String = msini.GetKeyValue("SystemSettings", "r.RefractionQuality")
        If c6 IsNot "" Then
            CheckBox6.Checked = True
        End If
        'pb_sysinfo.Value += 1

        'Dim c7 As String = msini.GetKeyValue("SystemSettings", "r.LightShaftQuality")
        Dim c7 As String = bsini.GetKeyValue("PostProcessQuality@0", "r.lightshafts")
        If c7 = "0" Then
            CheckBox7.Checked = True
        End If
        'pb_sysinfo.Value += 1

        Dim c8 As String = msini.GetKeyValue("SystemSettings", "r.LightFunctionQuality")
        If c8 IsNot "" Then
            CheckBox8.Checked = True
        End If
        'pb_sysinfo.Value += 1

        Dim c9 As String = msini.GetKeyValue("SystemSettings", "r.UpsampleQuality")
        If c9 IsNot "" Then
            CheckBox9.Checked = True
        End If
        'pb_sysinfo.Value += 1

        Dim c10 As String = msini.GetKeyValue("SystemSettings", "r.LensFlareQuality")
        If c10 IsNot "" Then
            CheckBox10.Checked = True
        End If
        'pb_sysinfo.Value += 1

        Dim c11 As String = msini.GetKeyValue("SystemSettings", "r.TranslucencyVolumeBlur")
        If c11 IsNot "" Then
            CheckBox11.Checked = True
        End If
        'pb_sysinfo.Value += 1

        Dim c12 As String = msini.GetKeyValue("SystemSettings", "r.EyeAdaptationQuality")
        If c12 IsNot "" Then
            CheckBox12.Checked = True
        End If
        'pb_sysinfo.Value += 1

        Dim c14 As String = msini.GetKeyValue("SystemSettings", "r.DepthOfFieldQuality")
        If c14 IsNot "" Then
            CheckBox14.Checked = True
        End If
        pb_sysinfo.Value = 4





        '''''GameSettings

        Dim cb1 As String = gsini.GetKeyValue("/Script/ShooterGame.ShooterGameUserSettings", "ActiveLingeringWorldTiles")
        If cb1 IsNot "" Then
            ComboBox1.SelectedIndex = (Convert.ToInt32(cb1) - 6) / 2
        End If
        'pb_sysinfo.Value += 1

        Dim cb2 As String = gsini.GetKeyValue("ScalabilityGroups", "sg.ViewDistanceQuality")
        If cb2 IsNot "" Then
            ComboBox2.SelectedIndex = Convert.ToInt32(cb2)
        End If
        'pb_sysinfo.Value += 1

        Dim cb3 As String = gsini.GetKeyValue("ScalabilityGroups", "sg.AntiAliasingQuality")
        If cb3 IsNot "" Then
            ComboBox3.SelectedIndex = Convert.ToInt32(cb3)
        End If
        'pb_sysinfo.Value += 1

        Dim cb4 As String = gsini.GetKeyValue("ScalabilityGroups", "sg.PostProcessQuality")
        If cb4 IsNot "" Then
            ComboBox4.SelectedIndex = Convert.ToInt32(cb4)
        End If
        'pb_sysinfo.Value += 1

        Dim cb5 As String = gsini.GetKeyValue("ScalabilityGroups", "sg.ShadowQuality")
        If cb5 IsNot "" Then
            ComboBox5.SelectedIndex = Convert.ToInt32(cb5)
        End If
        'pb_sysinfo.Value += 1

        Dim cb6 As String = gsini.GetKeyValue("ScalabilityGroups", "sg.HeightFieldShadowQuality")
        If cb6 IsNot "" Then
            ComboBox6.SelectedIndex = Convert.ToInt32(cb6)
        End If
        'pb_sysinfo.Value += 1

        Dim cb7 As String = gsini.GetKeyValue("ScalabilityGroups", "sg.TextureQuality")
        If cb7 IsNot "" Then
            ComboBox7.SelectedIndex = Convert.ToInt32(cb7)
        End If
        'pb_sysinfo.Value += 1

        Dim c15 As String = gsini.GetKeyValue("/Script/ShooterGame.ShooterGameUserSettings", "bMotionBlur")
        If c15 IsNot "" Then
            CheckBox15.Checked = Convert.ToBoolean(c15)
        End If
        'pb_sysinfo.Value += 1

        Dim c16 As String = gsini.GetKeyValue("/Script/ShooterGame.ShooterGameUserSettings", "bFilmGrain")
        If c16 IsNot "" Then
            CheckBox16.Checked = Convert.ToBoolean(c16)
        End If
        'pb_sysinfo.Value += 1

        Dim c17 As String = gsini.GetKeyValue("/Script/ShooterGame.ShooterGameUserSettings", "bUseDFAO")
        If c17 = "true" Then
            CheckBox17.Checked = Convert.ToBoolean(c17)
        End If
        'pb_sysinfo.Value += 1

        Dim c18 As String = gsini.GetKeyValue("/Script/ShooterGame.ShooterGameUserSettings", "bUseSSAO")
        If c18 IsNot "" Then
            CheckBox18.Checked = Convert.ToBoolean(c18)
        End If
        'pb_sysinfo.Value += 1

        'Dim c19 As String = gsini.GetKeyValue("/Script/ShooterGame.ShooterGameUserSettings", "bDistanceFieldShadowing")
        'If c19 IsNot "" Then
        'CheckBox19.Checked = True
        'End If
        'pb_sysinfo.Value += 1

        Dim c20 As String = gsini.GetKeyValue("/Script/ShooterGame.ShooterGameUserSettings", "bDistanceFieldShadowing")
        If c20 IsNot "" Then
            CheckBox20.Checked = Convert.ToBoolean(c20)
        End If
        'pb_sysinfo.Value += 1

        Dim c21 As String = gsini.GetKeyValue("/Script/ShooterGame.ShooterGameUserSettings", "HighQualityMaterials")
        If c21 IsNot "" Then
            CheckBox21.Checked = Convert.ToBoolean(c21)
        End If
        'pb_sysinfo.Value += 1

        Dim c22 As String = gsini.GetKeyValue("/Script/ShooterGame.ShooterGameUserSettings", "HighQualitySurfaces")
        If c22 IsNot "" Then
            CheckBox22.Checked = Convert.ToBoolean(c22)
        End If
        'pb_sysinfo.Value += 1

        Dim c23 As String = gsini.GetKeyValue("/Script/ShooterGame.ShooterGameUserSettings", "bUseVSync")
        If c23 IsNot "" Then
            CheckBox23.Checked = Convert.ToBoolean(c23)
        End If
        'pb_sysinfo.Value += 1

        Dim n1 As String = gsini.GetKeyValue("ScalabilityGroups", "sg.ResolutionQuality")
        If n1 IsNot "" Then
            If n1 > 100 Then
                n1 = 100
            End If
            NumericUpDown1.Value = Convert.ToInt32(n1)
        End If
        'pb_sysinfo.Value += 1

        Dim n2 As String = gsini.GetKeyValue("/Script/ShooterGame.ShooterGameUserSettings", "TrueSkyQuality")
        If n2 IsNot "" Then
            If n2 > 1 Then
                n2 = 1
            End If
            NumericUpDown2.Value = Convert.ToInt32(n2 * 100)
        End If
        'pb_sysinfo.Value += 1

        Dim n3 As String = gsini.GetKeyValue("/Script/ShooterGame.ShooterGameUserSettings", "GroundClutterDensity")
        If n3 IsNot "" Then
            If n3 > 1 Then
                n3 = 1
            End If
            NumericUpDown3.Value = Convert.ToInt32(n3 * 100)
        End If
        'pb_sysinfo.Value += 1

        Dim n4 As String = gsini.GetKeyValue("/Script/ShooterGame.ShooterGameUserSettings", "LODScalar")
        If n4 IsNot "" Then
            If (n4) > 1 Then
                n4 = 1
            End If
            NumericUpDown4.Value = Convert.ToInt32(n4 * 100)
        End If
        pb_sysinfo.Value = 5





        pb_sysinfo.Value = pb_sysinfo.Maximum


        Button1.Visible = True
    End Sub


    Private Sub SetSettings()


        pb_sysinfo.Value = 0
        If CheckBox1.Checked = True Then
            msini.SetKeyValue("SystemSettings", "r.PostProcessAAQuality", "0")
        Else
            msini.RemoveKey("SystemSettings", "r.PostProcessAAQuality")
        End If
        pb_sysinfo.Value = 1
        If CheckBox2.Checked = True Then
            msini.SetKeyValue("SystemSettings", "r.ShadowQuality", "0")
            msini.SetKeyValue("SystemSettings", "r.ShadowFilterQualityBias", "0")
            msini.SetKeyValue("SystemSettings", "r.Shadow.CSM.MaxCascades", "1")
            msini.SetKeyValue("SystemSettings", "r.Shadow.MaxResolution", "256")
            msini.SetKeyValue("SystemSettings", "r.Shadow.RadiusThreshold", "0.06")
            msini.SetKeyValue("SystemSettings", "r.Shadow.DistanceScale", "0.6")
            msini.SetKeyValue("SystemSettings", "r.Shadow.CSM.TransitionScale", "0")
        Else
            msini.RemoveKey("SystemSettings", "r.ShadowQuality")
            msini.RemoveKey("SystemSettings", "r.ShadowFilterQualityBias")
            msini.RemoveKey("SystemSettings", "r.Shadow.CSM.MaxCascades")
            msini.RemoveKey("SystemSettings", "r.Shadow.MaxResolution")
            msini.RemoveKey("SystemSettings", "r.Shadow.RadiusThreshold")
            msini.RemoveKey("SystemSettings", "r.Shadow.DistanceScale")
            msini.RemoveKey("SystemSettings", "r.Shadow.CSM.TransitionScale")
        End If
        'pb_sysinfo.Value += 1
        If CheckBox3.Checked = True Then
            msini.SetKeyValue("SystemSettings", "r.MotionBlurQuality", "0")
            msini.SetKeyValue("SystemSettings", "r.BlurGBuffer", "0")
            msini.SetKeyValue("SystemSettings", "r.FastBlurThreshold", "0")
        Else
            msini.RemoveKey("SystemSettings", "r.MotionBlurQuality")
            msini.RemoveKey("SystemSettings", "r.BlurGBuffer")
            msini.RemoveKey("SystemSettings", "r.FastBlurThreshold")
        End If
        'pb_sysinfo.Value += 1
        If CheckBox4.Checked = True Then
            msini.SetKeyValue("SystemSettings", "r.AmbientOcclusionLevels", "0")
            msini.SetKeyValue("SystemSettings", "r.AmbientOcclusionRadiusScale", "1.7")
        Else
            msini.RemoveKey("SystemSettings", "r.AmbientOcclusionLevels")
            msini.RemoveKey("SystemSettings", "r.AmbientOcclusionRadiusScale")
        End If
        'pb_sysinfo.Value += 1
        If CheckBox5.Checked = True Then
            bsini.SetKeyValue("SystemSettings", "r.DefaultFeature.Bloom", "False")
            bsini.SetKeyValue("PostProcessQuality@0", "r.BloomQuality", "0")
            bsini.SetKeyValue("PostProcessQuality@1", "r.BloomQuality", "0")
            bsini.SetKeyValue("PostProcessQuality@2", "r.BloomQuality", "0")
            bsini.SetKeyValue("PostProcessQuality@3", "r.BloomQuality", "0")
        Else
            bsini.SetKeyValue("PostProcessQuality@0", "r.BloomQuality", "2")
            bsini.SetKeyValue("PostProcessQuality@1", "r.BloomQuality", "4")
            bsini.SetKeyValue("PostProcessQuality@2", "r.BloomQuality", "5")
            bsini.SetKeyValue("PostProcessQuality@3", "r.BloomQuality", "5")
        End If
        'pb_sysinfo.Value += 1
        If CheckBox6.Checked = True Then
            msini.SetKeyValue("SystemSettings", "r.RefractionQuality", "0")
        Else
            msini.RemoveKey("SystemSettings", "r.RefractionQuality")
        End If
        'pb_sysinfo.Value += 1
        If CheckBox7.Checked = True Then
            bsini.SetKeyValue("PostProcessQuality@0", "r.lightshafts", "0")
            bsini.SetKeyValue("PostProcessQuality@1", "r.lightshafts", "0")
            bsini.SetKeyValue("PostProcessQuality@2", "r.lightshafts", "0")
            bsini.SetKeyValue("PostProcessQuality@3", "r.lightshafts", "0")
        Else
            bsini.RemoveKey("PostProcessQuality@0", "r.lightshafts")
            bsini.RemoveKey("PostProcessQuality@1", "r.lightshafts")
            bsini.RemoveKey("PostProcessQuality@2", "r.lightshafts")
            bsini.RemoveKey("PostProcessQuality@3", "r.lightshafts")
        End If
        'pb_sysinfo.Value += 1
        If CheckBox8.Checked = True Then
            msini.SetKeyValue("SystemSettings", "r.LightFunctionQuality", "0")
        Else
            msini.RemoveKey("SystemSettings", "r.LightFunctionQuality")
        End If
        'pb_sysinfo.Value += 1
        If CheckBox9.Checked = True Then
            msini.SetKeyValue("SystemSettings", "r.UpsampleQuality", "1")
        Else
            msini.RemoveKey("SystemSettings", "r.UpsampleQuality")
        End If
        'pb_sysinfo.Value += 1
        If CheckBox10.Checked = True Then
            msini.SetKeyValue("SystemSettings", "r.LensFlareQuality", "0")
            bsini.SetKeyValue("PostProcessQuality@0", "r.LensFlareQuality", "0")
            bsini.SetKeyValue("PostProcessQuality@1", "r.LensFlareQuality", "0")
            bsini.SetKeyValue("PostProcessQuality@2", "r.LensFlareQuality", "0")
            bsini.SetKeyValue("PostProcessQuality@3", "r.LensFlareQuality", "0")
        Else
            msini.RemoveKey("SystemSettings", "r.LensFlareQuality")
            bsini.RemoveKey("PostProcessQuality@0", "r.LensFlareQuality")
            bsini.RemoveKey("PostProcessQuality@1", "r.LensFlareQuality")
            bsini.RemoveKey("PostProcessQuality@2", "r.LensFlareQuality")
            bsini.RemoveKey("PostProcessQuality@3", "r.LensFlareQuality")
        End If
        'pb_sysinfo.Value += 1
        If CheckBox11.Checked = True Then
            msini.SetKeyValue("SystemSettings", "r.TranslucencyVolumeBlur", "0")
        Else
            msini.RemoveKey("SystemSettings", "r.TranslucencyVolumeBlur")
        End If
        'pb_sysinfo.Value += 1
        If CheckBox12.Checked = True Then
            msini.SetKeyValue("SystemSettings", "r.EyeAdaptationQuality", "0")
        Else
            msini.RemoveKey("SystemSettings", "r.EyeAdaptationQuality")
        End If
        'pb_sysinfo.Value += 1
        If CheckBox13.Checked = True Then
            msini.SetKeyValue("SystemSettings", "r.SSR", "0")
            msini.SetKeyValue("SystemSettings", "r.SSR.Quality", "0")
        Else
            msini.RemoveKey("SystemSettings", "r.SSR")
            msini.RemoveKey("SystemSettings", "r.SSR.Quality")
        End If
        'pb_sysinfo.Value += 1
        If CheckBox14.Checked = True Then
            msini.SetKeyValue("SystemSettings", "r.DepthOfFieldQuality", "1")
        Else
            msini.RemoveKey("SystemSettings", "r.DepthOfFieldQuality")
        End If
        pb_sysinfo.Value = 2





        gsini.SetKeyValue("/Script/ShooterGame.ShooterGameUserSettings", "ActiveLingeringWorldTiles", ((ComboBox1.SelectedIndex * 2) + 6))
        'pb_sysinfo.Value += 1
        gsini.SetKeyValue("ScalabilityGroups", "sg.ViewDistanceQuality", ComboBox2.SelectedIndex)
        'pb_sysinfo.Value += 1
        gsini.SetKeyValue("ScalabilityGroups", "sg.AntiAliasingQuality", ComboBox3.SelectedIndex)
        'pb_sysinfo.Value += 1
        gsini.SetKeyValue("ScalabilityGroups", "sg.PostProcessQuality", ComboBox4.SelectedIndex)
        'pb_sysinfo.Value += 1
        gsini.SetKeyValue("ScalabilityGroups", "sg.ShadowQuality", ComboBox5.SelectedIndex)
        'pb_sysinfo.Value += 1
        gsini.SetKeyValue("ScalabilityGroups", "sg.HeightFieldShadowQuality", ComboBox6.SelectedIndex)
        'pb_sysinfo.Value += 1
        gsini.SetKeyValue("ScalabilityGroups", "sg.TextureQuality", ComboBox7.SelectedIndex)
        'pb_sysinfo.Value += 1

        gsini.SetKeyValue("/Script/ShooterGame.ShooterGameUserSettings", "bMotionBlur", CheckBox15.Checked)
        'pb_sysinfo.Value += 1
        gsini.SetKeyValue("/Script/ShooterGame.ShooterGameUserSettings", "bFilmGrain", CheckBox15.Checked)
        'pb_sysinfo.Value += 1
        gsini.SetKeyValue("/Script/ShooterGame.ShooterGameUserSettings", "bUseDFAO", CheckBox15.Checked)
        'pb_sysinfo.Value += 1
        gsini.SetKeyValue("/Script/ShooterGame.ShooterGameUserSettings", "bUseSSAO", CheckBox15.Checked)
        'pb_sysinfo.Value += 1
        'gsini.SetKeyValue("/Script/ShooterGame.ShooterGameUserSettings", "tesselation", CheckBox19.CheckState)
        'pb_sysinfo.Value += 1
        gsini.SetKeyValue("/Script/ShooterGame.ShooterGameUserSettings", "bDistanceFieldShadowing", CheckBox20.Checked)
        'pb_sysinfo.Value += 1
        gsini.SetKeyValue("/Script/ShooterGame.ShooterGameUserSettings", "HighQualityMaterials", CheckBox21.Checked)
        'pb_sysinfo.Value += 1
        gsini.SetKeyValue("/Script/ShooterGame.ShooterGameUserSettings", "HighQualitySurfaces", CheckBox22.Checked)
        'pb_sysinfo.Value += 1
        gsini.SetKeyValue("/Script/ShooterGame.ShooterGameUserSettings", "bMobUseVSynctionBlur", CheckBox23.Checked)
        'pb_sysinfo.Value += 1

        gsini.SetKeyValue("ScalabilityGroups", "sg.ResolutionQuality", (NumericUpDown1.Value))
        'pb_sysinfo.Value += 1
        gsini.SetKeyValue("/Script/ShooterGame.ShooterGameUserSettings", "TrueSkyQuality", (NumericUpDown2.Value / 100))
        'pb_sysinfo.Value += 1
        gsini.SetKeyValue("/Script/ShooterGame.ShooterGameUserSettings", "GroundClutterDensity", (NumericUpDown3.Value / 100))
        'pb_sysinfo.Value += 1
        gsini.SetKeyValue("/Script/ShooterGame.ShooterGameUserSettings", "LODScalar", (NumericUpDown4.Value / 100))
        pb_sysinfo.Value = 3








        msini.Save(msfile)
        gsini.Save(gsfile)
        'MsgBox(bsfile)
        bsini.Save(bsfile)
        pb_sysinfo.Value = pb_sysinfo.Maximum
    End Sub


    Private Sub GetGraphicsCardName()
        Dim query As New System.Management.SelectQuery("Win32_VideoController")
        Dim search As New System.Management.ManagementObjectSearcher(query)
        Dim info As System.Management.ManagementObject

        Dim gpu As String = "..."
        Dim gpuram As Int32 = 0
        Dim gpudvr As String = "..."
        'MsgBox(search.Get.Count)
        For Each info In search.Get()
            gpu = info("Name").ToString
            'info("VideoProcessor").ToString()
            gpuram = info("AdapterRAM") / 1024 / 1024
            gpudvr = info("DriverVersion").ToString
            DataGridView1.Rows.Add(New String() {gpu.ToString, gpuram.ToString, gpudvr.ToString})
            l_gpu.Items.Insert(l_gpu.Items.Count, gpu.ToString)

        Next

        If Not arkegpu = "" Then
            l_gpu.Text = DataGridView1.Rows(arkegpu).Cells(0).Value
            l_gpuram.Text = DataGridView1.Rows(arkegpu).Cells(1).Value & " MB"
            l_gpudvr.Text = DataGridView1.Rows(arkegpu).Cells(2).Value
        Else
            l_gpu.Text = gpu
            l_gpuram.Text = gpuram & " MB"
            l_gpudvr.Text = gpudvr
        End If

        'l_gpu_o.Text = gpu
        'l_gpuram.Text = gpuram & " MB"
        'l_gpudvr.Text = gpudvr
        'Return GraphicsCardName
    End Sub
    Private Sub GetCPUName()
        Dim query As New System.Management.SelectQuery("Win32_Processor")
        Dim search As New System.Management.ManagementObjectSearcher(query)
        Dim info As System.Management.ManagementObject

        Dim gpu As String = "..."
        Dim gpuram As String = "..."
        Dim gpudvr As Int32 = 0
        Dim gputhd As Int32 = My.Computer.Info.TotalPhysicalMemory.ToString / 1024 / 1024

        For Each info In search.Get()
            gpu = info("Name").ToString
            'info("VideoProcessor").ToString()
            gpuram = info("NumberOfCores").ToString
            gpudvr = info("MaxClockSpeed")
            'gputhd = info("ThreadCount").ToString
        Next

        l_cpu.Text = gpu
        l_cpucre.Text = gpuram
        l_cpuspd.Text = gpudvr & " MHz"
        'l_cputhd.Text = gputhd
        l_mem.Text = gputhd & " MB"
        'Return GraphicsCardName
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        MsgBox("This setting causes ALL light sources to emit no light." & vbCrLf & "Torches, Campfires, Standing Torches, etc." & vbCrLf & "GET SAFE AT NIGHT!")
    End Sub

    Private Sub LinkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        MsgBox("ARKE cannot edit Steam Client settings." & vbCrLf & "Please go to ark.hiveserver.net/commandflags for a guide on how to set command flags for ARK")
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click


        If (CheckBox24.Checked) Then
            Dim d As DateTime = Now
            'arke-2015-6-24-18-48-12
            Dim stamptext = "-arke-" & d.ToString("yyyy-MM-dd-HH-MM-ss")
            Dim msbfile As String = msfile.Insert(msfile.Length - 4, stamptext)
            Dim gsbfile As String = gsfile.Insert(gsfile.Length - 4, stamptext)
            Dim bsbfile As String = bsfile.Insert(bsfile.Length - 4, stamptext)
            msini.Save(msbfile)
            gsini.Save(gsbfile)
            bsini.Save(bsbfile)
        End If

        SetSettings()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim onyn = MsgBox("Do you want to let ArkEnhancer connect to ark.hiveserver.net to check for compatability settings for your graphics card?", MsgBoxStyle.YesNo, "Online Access")
        If onyn = MsgBoxResult.Yes Then
            Dim request As WebRequest = WebRequest.Create("http://ark.hiveserver.net/arkespecs.php?card=" & l_gpu.Text & "&proc=" & l_cpu.Text)
            Dim response As WebResponse = request.GetResponse()
            Dim dataStream As Stream = response.GetResponseStream()
            Dim reader As New StreamReader(dataStream)
            Dim responseFromServer As String = reader.ReadToEnd()
            'Console.WriteLine(responseFromServer)
            reader.Close()
            response.Close()

            'MsgBox(responseFromServer)
            If Not responseFromServer = "none" Then
                Dim csv As New List(Of String)(responseFromServer.Split(","c))
                'Dim csv() As String = responseFromServer.Split(","c)
                'Dim dt As New DataTable
                'dt.Columns.Add("1", GetType(String))
                ' Dim newrow As DataRow = dt.NewRow
                'MsgBox(responseFromServer)
                'newrow.ItemArray = {csv(0), csv(1), csv(2), csv(3), csv(4), csv(5), csv(6), csv(7), csv(8), csv(9), csv(10), csv(11), csv(12), csv(13)}
                'newrow.ItemArray = {csv(0)}
                'dt.Rows.Add(newrow)
                'DataGridView1.DataSource = dt
                'If csv(0) = "1" Then
                'End If
                CheckBox1.Checked = Convert.ToInt32(csv(0))
                CheckBox2.Checked = Convert.ToInt32(csv(1))
                CheckBox3.Checked = Convert.ToInt32(csv(2))
                CheckBox4.Checked = Convert.ToInt32(csv(3))
                CheckBox5.Checked = Convert.ToInt32(csv(4))
                CheckBox6.Checked = Convert.ToInt32(csv(5))
                CheckBox7.Checked = Convert.ToInt32(csv(6))
                CheckBox8.Checked = Convert.ToInt32(csv(7))
                CheckBox9.Checked = Convert.ToInt32(csv(8))
                CheckBox10.Checked = Convert.ToInt32(csv(9))
                CheckBox11.Checked = Convert.ToInt32(csv(10))
                CheckBox12.Checked = Convert.ToInt32(csv(11))
                CheckBox13.Checked = Convert.ToInt32(csv(12))
                CheckBox14.Checked = Convert.ToInt32(csv(13))

                TextBox1.Text = csv(14)

                NumericUpDown1.Value = Convert.ToInt32(csv(15))
                ComboBox1.Text = ComboBox1.Items(Convert.ToInt32(csv(16)) - 1)
                ComboBox2.Text = ComboBox1.Items(Convert.ToInt32(csv(17)) - 1)
                ComboBox3.Text = ComboBox1.Items(Convert.ToInt32(csv(18)) - 1)
                ComboBox4.Text = ComboBox1.Items(Convert.ToInt32(csv(19)) - 1)
                ComboBox5.Text = ComboBox1.Items(Convert.ToInt32(csv(20)) - 1)
                ComboBox6.Text = ComboBox1.Items(Convert.ToInt32(csv(21)) - 1)
                ComboBox7.Text = ComboBox1.Items(Convert.ToInt32(csv(22)) - 1)
                NumericUpDown2.Value = Convert.ToInt32(csv(23))
                NumericUpDown3.Value = Convert.ToInt32(csv(24))
                CheckBox15.Checked = Convert.ToInt32(csv(25))
                CheckBox16.Checked = Convert.ToInt32(csv(26))
                CheckBox17.Checked = Convert.ToInt32(csv(27))
                CheckBox18.Checked = Convert.ToInt32(csv(28))
                CheckBox19.Checked = Convert.ToInt32(csv(29))
                CheckBox20.Checked = Convert.ToInt32(csv(30))
                CheckBox21.Checked = Convert.ToInt32(csv(31))
                CheckBox22.Checked = Convert.ToInt32(csv(32))
                NumericUpDown4.Value = Convert.ToInt32(csv(33))

            Else
                MsgBox("No optimal settings found for your graphics card.")
            End If

        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim submitstring As String = l_cpu.Text & "," & l_cpuspd.Text & "," & l_mem.Text & "," & l_gpu.Text & "," & l_gpuram.Text & "," & l_gpudvr.Text & "," & CheckBox1.CheckState & "," & CheckBox2.CheckState & "," & CheckBox3.CheckState & "," & CheckBox4.CheckState & "," & CheckBox5.CheckState & "," & CheckBox6.CheckState & "," & CheckBox7.CheckState & "," & CheckBox8.CheckState & "," & CheckBox9.CheckState & "," & CheckBox10.CheckState & "," & CheckBox11.CheckState & "," & CheckBox12.CheckState & "," & CheckBox13.CheckState & "," & CheckBox14.CheckState & "," & TextBox1.Text & "," & NumericUpDown1.Value & "," & (ComboBox1.SelectedIndex + 1) & "," & (ComboBox2.SelectedIndex + 1) & "," & (ComboBox3.SelectedIndex + 1) & "," & (ComboBox4.SelectedIndex + 1) & "," & (ComboBox5.SelectedIndex + 1) & "," & (ComboBox6.SelectedIndex + 1) & "," & (ComboBox7.SelectedIndex + 1) & "," & NumericUpDown2.Value & "," & NumericUpDown3.Value & "," & CheckBox15.CheckState & "," & CheckBox16.CheckState & "," & CheckBox17.CheckState & "," & CheckBox18.CheckState & "," & CheckBox19.CheckState & "," & CheckBox20.CheckState & "," & CheckBox21.CheckState & "," & CheckBox22.CheckState & "," & NumericUpDown4.Value & "," & CheckBox23.CheckState
        Dim onyn = MsgBox("Do you want to let ArkEnhancer connect to ark.hiveserver.net submit your settings?" & vbCrLf & "ARKE will only submit data visible for comparison and data entry." & vbCrLf & vbCrLf & submitstring, MsgBoxStyle.YesNo, "Online Access")
        If onyn = MsgBoxResult.Yes Then
            Dim request As WebRequest = WebRequest.Create("http://ark.hiveserver.net/arkesubmit.php?data=" & submitstring)
            Dim response As WebResponse = request.GetResponse()
            Dim dataStream As Stream = response.GetResponseStream()
            Dim reader As New StreamReader(dataStream)
            Dim responseFromServer As String = reader.ReadToEnd()
            'Console.WriteLine(responseFromServer)
            reader.Close()
            response.Close()
            MsgBox(responseFromServer)
        End If
    End Sub

    Private Sub l_gpu_SelectedIndexChanged(sender As Object, e As EventArgs) Handles l_gpu.SelectedIndexChanged
        l_gpu.Text = DataGridView1.Rows(l_gpu.SelectedIndex).Cells(0).Value
        l_gpuram.Text = DataGridView1.Rows(l_gpu.SelectedIndex).Cells(1).Value & " MB"
        l_gpudvr.Text = DataGridView1.Rows(l_gpu.SelectedIndex).Cells(2).Value

        arkessini.SetKeyValue("SystemSettings", "selgpuindex", l_gpu.SelectedIndex)
        arkessini.Save(localfolder & "\arkespecialsettings.ini")
    End Sub

    Private Sub CheckBox24_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox24.Click
        arkessini.SetKeyValue("AppSettings", "makebackup", CheckBox24.Checked)
        arkessini.Save(localfolder & "\arkespecialsettings.ini")
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Form2.ShowDialog()

    End Sub

    Private Sub LinkLabel3_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel3.LinkClicked, LinkLabel4.LinkClicked
        'MsgBox("As of current game version 183.1, this setting does absolutely nothing." & vbCrLf & "I am leaving this option available in case the developers fix it.")
        MsgBox("This option is experimental in this version of ARK Enhancer")
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        Form3.ShowDialog()
    End Sub

    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click
        Dim filespace = "http://ark.hiveserver.net/"
        Process.Start(filespace)
    End Sub



    Public Sub CheckForUpdate()
        ''Dim submitstring As String = l_cpu.Text & "," & l_cpuspd.Text & "," & l_mem.Text & "," & l_gpu.Text & "," & l_gpuram.Text & "," & l_gpudvr.Text & "," & CheckBox1.CheckState & "," & CheckBox2.CheckState & "," & CheckBox3.CheckState & "," & CheckBox4.CheckState & "," & CheckBox5.CheckState & "," & CheckBox6.CheckState & "," & CheckBox7.CheckState & "," & CheckBox8.CheckState & "," & CheckBox9.CheckState & "," & CheckBox10.CheckState & "," & CheckBox11.CheckState & "," & CheckBox12.CheckState & "," & CheckBox13.CheckState & "," & CheckBox14.CheckState & "," & TextBox1.Text & "," & NumericUpDown1.Value & "," & (ComboBox1.SelectedIndex + 1) & "," & (ComboBox2.SelectedIndex + 1) & "," & (ComboBox3.SelectedIndex + 1) & "," & (ComboBox4.SelectedIndex + 1) & "," & (ComboBox5.SelectedIndex + 1) & "," & (ComboBox6.SelectedIndex + 1) & "," & (ComboBox7.SelectedIndex + 1) & "," & NumericUpDown2.Value & "," & NumericUpDown3.Value & "," & CheckBox15.CheckState & "," & CheckBox16.CheckState & "," & CheckBox17.CheckState & "," & CheckBox18.CheckState & "," & CheckBox19.CheckState & "," & CheckBox20.CheckState & "," & CheckBox21.CheckState & "," & CheckBox22.CheckState & "," & NumericUpDown4.Value & "," & CheckBox22.CheckState
        'Dim onyn = MsgBox("Do you want to let ArkEnhancer connect to ark.hiveserver.net submit your settings?" & vbCrLf & "ARKE will only submit data visible for comparison and data entry." & vbCrLf & vbCrLf & submitstring, MsgBoxStyle.YesNo, "Online Access")
        'If onyn = MsgBoxResult.Yes Then
        Dim request As WebRequest = WebRequest.Create("http://ark.hiveserver.net/arkeupdate_getversion.php?current=" & ARKE_VNum & "&line=" & ARKE_VLin)
        Dim response As WebResponse = request.GetResponse()
            Dim dataStream As Stream = response.GetResponseStream()
            Dim reader As New StreamReader(dataStream)
            Dim responseFromServer As String = reader.ReadToEnd()
            'Console.WriteLine(responseFromServer)
            reader.Close()
            response.Close()
        'MsgBox(responseFromServer)
        If (responseFromServer) Then
            Dim avl = "stable"
            If ARKE_VLin = 1 Then
                avl = "experimental"
            End If
            'Dim filespace = "http://ark.hiveserver.net/files/arke_" & avl & ".zip"
            Dim filespace = "http://ark.hiveserver.net/getarke?v=" & ARKE_VLin
            Dim onyn = MsgBox("An update is available." & vbCrLf & "Do you want ArkEnhancer to download the below file?" & vbCrLf & vbCrLf & filespace & vbCrLf & vbCrLf & "Pressing [Yes] will open your default browser.", MsgBoxStyle.YesNo, "Online Access")
            If onyn = MsgBoxResult.Yes Then
                Process.Start(filespace)
            End If
        End If
        ARKE_VLUT = Date.Now
        arkessini.SetKeyValue("AppSettings", "UpdateLast", ARKE_VLUT)
        arkessini.Save(localfolder & "\arkespecialsettings.ini")
        'End If
    End Sub
End Class
