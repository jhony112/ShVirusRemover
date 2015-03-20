'**************************************************************'
' Component ProgressbarWithPercentage v1.0.10 - November 2009  '
' By Jeroen De Dauw - jeroendedauw@gmail.com                   '
' Based on code by people at vb.net forums                     '
'**************************************************************'

' A description of this class, usage instructions, and examples can be found at The Code project:
' http://www.codeproject.com/KB/progress/progressbar-percentage.aspx

' This code is avaible at
' > SourceForge     : https://sourceforge.net/projects/pprogressbar/files/
' > BN+ Discussions : http://code.bn2vs.com/forum/viewtopic.php?t=134
' > The Code Project: http://www.codeproject.com/KB/progress/progressbar-percentage.aspx

' C#.Net implementation by DaveyM69
' > http://www.codeproject.com/KB/progress/progressbar-percentage.aspx?fid=1537063&df=90&mpp=25&noise=3&sort=Position&view=None&select=2959781#xx2959781xx

Option Strict On : Option Explicit On

Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms

#Region " Public Class ProgressbarWithPercentage "
''' <summary>Component that extends the native .net progressbar with percentage properties and the ability to overlay the percentage</summary>
''' <remarks>Component ProgressbarWithPercentage v1.0.10, by De Dauw Jeroen - November 2009</remarks>
<DesignTimeVisible(True), DefaultProperty("Value"), DefaultEvent("ValueChanged"), _
Description("Component that extends the native .net progressbar with percentage properties and the ability to overlay the percentage")> _
Public Class ProgressbarWithPercentage
    Inherits System.Windows.Forms.ProgressBar

#Region "Events"
    ''' <summary>Occurs when the value of the progress bar is changed</summary>
    <Category("Property Changed")> _
    Public Event ValueChanged As EventHandler
    ''' <summary>Occurs when the amount of decimals to be displayed in the percentage is changed</summary>
    <Category("Property Changed")> _
    Public Event PercentageDecimalsChanged As EventHandler
    ''' <summary>Occurs when the visibility of the percentage text is changed</summary>
    <Category("Property Changed")> _
    Public Event PercentageVisibleChanged As EventHandler
    ''' <summary>Occurs when the automatic updating of the percentage is turned on or off</summary>
    <Category("Property Changed")> _
    Public Event AutoUpdatePercentageChanged As EventHandler
    ''' <summary>Occurs when the OverlayTextColor property is changed</summary>
    <Category("Property Changed")> _
    Public Event OverlayTextColorChanged As EventHandler
    ''' <summary>Occurs when the TextColor property is changed</summary>
    <Category("Property Changed")> _
    Public Event TextColorChanged As EventHandler
    ''' <summary>Occurs when the align of the percentage text is changed</summary>
    <Category("Property Changed")> _
    Public Event PercentageAlignChanged As EventHandler
    ''' <summary>Occurs when the display format is changed</summary>
    <Category("Property Changed")> _
    Public Event DisplayFormatChanged As EventHandler
    ''' <summary>Occurs when the padding of the percentage text is changed</summary>
    <Category("Property Changed")> _
    Public Shadows Event PaddingChanged As EventHandler
#End Region

#Region "Fields"
    Private Const WM_PAINT As Int32 = &HF 'hex for 15

    Private m_auto_update, m_p_visible As Boolean
    Private m_displayFormat As String
    Private m_decimals As Int32
    Private m_p_align As ContentAlignment
    Private m_graphics As Graphics
    Private m_textColor, m_overlayTextColor As Color
    Private m_drawingRectangle As RectangleF
    Private m_strFormat As New StringFormat
#End Region

#Region "Public methods"
    ''' <summary>Create a new instance of a ProgressbarWithPercentage</summary>
    Public Sub New()
        ' Initialize the base class
        MyBase.New()

        ' Set the default values of the properties
        Me.DisplayFormat = "{0}%"
        Me.AutoUpdatePercentage = True
        Me.PercentageVisible = True
        Me.PercentageDecimals = 0
        Me.PercentageAlign = ContentAlignment.MiddleCenter
        Me.TextColor = Color.DimGray
        Me.OverlayTextColor = Color.Black
        Me.Style = ProgressBarStyle.Continuous

        ' Calculate the initial gfx related values
        setGfx()
        setStringFormat()
        setDrawingRectangle()
    End Sub

    ''' <summary>Advances the current possition of the progressbar by the amount of the Step property</summary>
    Public Shadows Sub PerformStep()
        MyBase.PerformStep()
        If Me.PercentageVisible And Me.AutoUpdatePercentage Then Me.ShowPercentage()
    End Sub

    ''' <summary>Show the current percentage as text</summary>
    Public Sub ShowPercentage()
        Me.ShowText(String.Format(Me.DisplayFormat, Math.Round(Me.Percentage, Me.PercentageDecimals).ToString))
    End Sub

    ''' <summary>Display a string on the progressbar</summary>
    ''' <param name="text">Required. String. The string to display</param>
    Public Sub ShowText(ByVal text As String)
        ' If the style is not marquee, regions will be used to allow the overlay color to be used
        If Me.Style <> ProgressBarStyle.Marquee Then
            ' Determine the areas for the ForeColor and OverlayColor
            Dim r1 As RectangleF = Me.ClientRectangle
            r1.Width = CSng(r1.Width * Me.Value / Me.Maximum)
            Dim reg1 As New Region(r1)
            Dim reg2 As New Region(Me.ClientRectangle)
            reg2.Exclude(reg1)

            ' Draw the string with the correct color depending on the region
            Me.Graphics.Clip = reg1
            Me.Graphics.DrawString(text, Me.Font, New SolidBrush(Me.OverlayTextColor), Me.DrawingRectangle, m_strFormat)
            Me.Graphics.Clip = reg2
            Me.Graphics.DrawString(text, Me.Font, New SolidBrush(Me.TextColor), Me.DrawingRectangle, m_strFormat)

            ' Dispose the regions
            reg1.Dispose()
            reg2.Dispose()
        Else
            ' Draw the string
            Me.Graphics.DrawString(text, Me.Font, New SolidBrush(Me.TextColor), Me.DrawingRectangle, m_strFormat)
        End If
    End Sub
#End Region

#Region "Protected methods"
    Protected Overrides Sub OnHandleCreated(ByVal e As System.EventArgs)
        MyBase.OnHandleCreated(e)
        Me.Graphics = Graphics.FromHwnd(Me.Handle)
    End Sub

    Protected Overrides Sub OnHandleDestroyed(ByVal e As System.EventArgs)
        Me.Graphics.Dispose()
        MyBase.OnHandleDestroyed(e)
    End Sub

    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        MyBase.WndProc(m)
        If m.Msg = WM_PAINT And Me.PercentageVisible And Me.AutoUpdatePercentage Then ShowPercentage()
    End Sub

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Me.AutoUpdatePercentage = False
        If disposing Then
            Me.Graphics.Dispose()
            m_strFormat.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub
#End Region

#Region "Private methods"
    Private Sub ProgressbarWithPercentage_SizeChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.SizeChanged
        setDrawingRectangle()
        setGfx()
    End Sub

    Private Sub ProgressbarWithPercentage_PaddingChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.PaddingChanged
        setDrawingRectangle()
    End Sub

    Private Sub setGfx()
        ' Set the graphics object
        Me.Graphics = Me.CreateGraphics
    End Sub

    Private Sub setDrawingRectangle()
        ' Determine the coordinates and size of the drawing rectangle depending on the progress bar size and padding
        Me.DrawingRectangle = New RectangleF(Me.Padding.Left, _
                                   Me.Padding.Top, _
                                   Me.Width - Me.Padding.Left - Me.Padding.Right, _
                                   Me.Height - Me.Padding.Top - Me.Padding.Bottom)
    End Sub

    Private Sub setStringFormat()
        ' Determine the horizontal alignment
        Select Case Me.PercentageAlign
            Case ContentAlignment.BottomCenter, ContentAlignment.BottomLeft, ContentAlignment.BottomRight
                m_strFormat.LineAlignment = StringAlignment.Far
            Case ContentAlignment.MiddleCenter, ContentAlignment.MiddleLeft, ContentAlignment.MiddleRight
                m_strFormat.LineAlignment = StringAlignment.Center
            Case ContentAlignment.TopCenter, ContentAlignment.TopLeft, ContentAlignment.TopRight
                m_strFormat.LineAlignment = StringAlignment.Near
        End Select

        ' Determine the vertical alignment
        Select Case Me.PercentageAlign
            Case ContentAlignment.BottomLeft, ContentAlignment.MiddleLeft, ContentAlignment.TopLeft
                m_strFormat.Alignment = StringAlignment.Near
            Case ContentAlignment.BottomCenter, ContentAlignment.MiddleCenter, ContentAlignment.TopCenter
                m_strFormat.Alignment = StringAlignment.Center
            Case ContentAlignment.BottomRight, ContentAlignment.MiddleRight, ContentAlignment.TopRight
                m_strFormat.Alignment = StringAlignment.Far
        End Select
    End Sub
#End Region

#Region "Properties"

#Region "Appearance"
    <Browsable(True), Category("Appearance"), Description("The value of the progressbar")> _
    Public Shadows Property Value() As Int32
        Get
            Return MyBase.Value
        End Get
        Set(ByVal value As Int32)
            If value <> Me.Value Then
                MyBase.Value = value
                If Me.PercentageVisible And Me.AutoUpdatePercentage Then
                    If Me.VistaVisualStylesEnabled Then
                        Me.Invalidate()
                    Else
                        Me.ShowPercentage()
                    End If
                End If
                RaiseEvent ValueChanged(Me, New EventArgs)
            End If
        End Set
    End Property

    <Browsable(True), Category("Appearance"), Description("The percentage of the progressbar")> _
    Public Property Percentage() As Double
        Get
            Return Me.Value / Me.Maximum * 100
        End Get
        Set(ByVal value As Double)
            If value >= 0 And value <= 100 Then
                Me.Value = CInt(Me.Maximum * value / 100)
            Else
                Throw New Exception("The percentage can not be needs to be equal or greather then 0 and smaller then or equal to 100")
            End If
        End Set
    End Property

    <Browsable(True), Category("Appearance"), DefaultValue(0), Description("Gets or sets the amount of decimals that will be displayed in the percentage")> _
    Public Overridable Property PercentageDecimals() As Int32
        Get
            Return m_decimals
        End Get
        Set(ByVal value As Int32)
            If value > -1 And value <> Me.PercentageDecimals Then
                m_decimals = value
                RaiseEvent PercentageDecimalsChanged(Me, New EventArgs)
            End If
        End Set
    End Property

    <Browsable(True), Category("Appearance"), Description("Gets or sets the font of the percentage text")> _
    Public Overridable Overloads Property Font() As Font
        Get
            Return MyBase.Font
        End Get
        Set(ByVal value As Font)
            MyBase.Font = value
        End Set
    End Property

    <Browsable(True), Category("Appearance"), DefaultValue("MiddleCenter"), Description("Gets or sets if the percentage alignment")> _
    Public Overridable Property PercentageAlign() As ContentAlignment
        Get
            Return m_p_align
        End Get
        Set(ByVal value As ContentAlignment)
            If value <> Me.PercentageAlign Then
                m_p_align = value
                setStringFormat()
                RaiseEvent PercentageAlignChanged(Me, New EventArgs)
            End If
        End Set
    End Property

    <Browsable(True), Category("Appearance"), Description("Gets or sets the color of the percentage text where the progressbar is not indicated")> _
    Public Overridable Property TextColor() As Color
        Get
            Return m_textColor
        End Get
        Set(ByVal value As Color)
            If Me.TextColor <> value Then
                m_textColor = value
                RaiseEvent TextColorChanged(Me, New EventArgs)
            End If
        End Set
    End Property

    <Browsable(True), Category("Appearance"), Description("Gets or sets the color of the percentage text where the progressbar is indicated")> _
    Public Overridable Property OverlayTextColor() As Color
        Get
            Return m_overlayTextColor
        End Get
        Set(ByVal value As Color)
            If Me.OverlayTextColor <> value Then
                m_overlayTextColor = value
                RaiseEvent OverlayTextColorChanged(Me, New EventArgs)
            End If
        End Set
    End Property

    <Browsable(True), Category("Appearance"), DefaultValue(True), Description("Gets or sets if the percentage should be visible")> _
    Public Overridable Property PercentageVisible() As Boolean
        Get
            Return m_p_visible
        End Get
        Set(ByVal value As Boolean)
            If value <> Me.PercentageVisible Then
                If Not value Then Me.Graphics.Clear(Me.BackColor)
                m_p_visible = value
                RaiseEvent PercentageVisibleChanged(Me, New EventArgs)
            End If
        End Set
    End Property

    <Browsable(True), Category("Appearance"), DefaultValue("{0}%"), Description("Gets or sets the display format of the percentage")> _
    Public Property DisplayFormat() As String
        Get
            Return m_displayFormat
        End Get
        Set(ByVal value As String)
            If value <> Me.DisplayFormat Then
                m_displayFormat = value
                Me.Invalidate()
                RaiseEvent DisplayFormatChanged(Me, New EventArgs)
            End If
        End Set
    End Property
#End Region

#Region "Behavior"
    <Browsable(True), Category("Behavior"), DefaultValue(True), Description("Gets or sets if the percentage should be auto updated")> _
    Public Overridable Property AutoUpdatePercentage() As Boolean
        Get
            Return m_auto_update
        End Get
        Set(ByVal value As Boolean)
            If value <> Me.AutoUpdatePercentage Then
                m_auto_update = value
                RaiseEvent AutoUpdatePercentageChanged(Me, New EventArgs)
            End If
        End Set
    End Property
#End Region

#Region "Layout"
    <Browsable(True), Category("Layout"), Description("Gets or sets if the interior spacing of the control")> _
    Public Overridable Overloads Property Padding() As Padding
        Get
            Return MyBase.Padding
        End Get
        Set(ByVal value As Padding)
            MyBase.Padding = value
        End Set
    End Property
#End Region

#Region "Misc"
    Protected Overridable Property Graphics() As Graphics
        Get
            Return m_graphics
        End Get
        Set(ByVal value As Graphics)
            If Me.Graphics IsNot Nothing Then Me.Graphics.Dispose()
            m_graphics = value
        End Set
    End Property

    Private Property DrawingRectangle() As RectangleF
        Get
            Return m_drawingRectangle
        End Get
        Set(ByVal value As RectangleF)
            m_drawingRectangle = value
        End Set
    End Property

    Private ReadOnly Property VistaVisualStylesEnabled() As Boolean
        Get
            Return Environment.OSVersion.Version.Major = 6 And Application.VisualStyleState <> VisualStyles.VisualStyleState.NoneEnabled
        End Get
    End Property
#End Region

#End Region

#Region "Designer"
    Private Sub InitializeComponent()
        Me.SuspendLayout()
        Me.ResumeLayout(False)
    End Sub
#End Region

End Class
#End Region