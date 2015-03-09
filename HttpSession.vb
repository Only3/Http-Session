    Class HttpSession

        Public File As String = "Session.cookie"
        Public Cookies As New Net.CookieContainer
        Public Headers As New Dictionary(Of String, String)
        Public UserAgent As String
        Public ContentType As String
        Public Redirect As Boolean = True
        Public Referer As String = Nothing
        Public KeepAlive As Boolean = False

        Sub New()
            ContentType = "application/x-www-form-urlencoded"
            UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/40.0.2214.115 Safari/537.36"
        End Sub

        Function [GET](Url As String, Optional Proxy As String = Nothing) As String
            Dim R = Request(Url, "GET")
            If Proxy IsNot Nothing Then R.Proxy = New Net.WebProxy(Proxy)
            Using Sr As New IO.StreamReader(R.GetResponse.GetResponseStream)
                Return Sr.ReadToEnd()
            End Using
        End Function

        Function POST(Url As String, Data As String, Optional Proxy As String = Nothing) As String
            Dim R = Request(Url, "POST")
            R.ContentLength = Data.Length
            If Proxy IsNot Nothing Then R.Proxy = New Net.WebProxy(Proxy)
            Dim S = R.GetRequestStream()
            Dim D = System.Text.Encoding.UTF8.GetBytes(Data)
            S.Write(D, 0, D.Length)
            S.Dispose()
            Using Sr As New IO.StreamReader(R.GetResponse.GetResponseStream)
                Return Sr.ReadToEnd()
            End Using
        End Function

        Function Request(Url As String, Method As String) As Net.HttpWebRequest
            Dim R = DirectCast(Net.WebRequest.Create(Url), Net.HttpWebRequest)
            R.Method = Method
            R.Referer = Referer
            R.UserAgent = UserAgent
            R.KeepAlive = KeepAlive
            R.ContentType = ContentType
            R.AllowAutoRedirect = Redirect
            R.CookieContainer = Cookies
            For Each K In Headers
                R.Headers.Set(K.Key, K.Value)
            Next
            Return R
        End Function

        Function Encode(Str As String) As String
            Return Uri.EscapeDataString(Str)
        End Function

        Function Decode(Str As String) As String
            Return Uri.UnescapeDataString(Str)
        End Function

        Sub NewSession(Optional Refer As Boolean = True)
            If Refer Then Referer = Nothing
            Cookies = New Net.CookieContainer
        End Sub

        Function SaveSession()
            Using F = IO.File.Create(File)
                Call New Runtime.Serialization.Formatters.Binary.BinaryFormatter().Serialize(F, Cookies)
                Return True
            End Using
            Return False
        End Function

        Function LoadSession() As Net.CookieContainer
            Using F = IO.File.Open(File, IO.FileMode.Open)
                Return TryCast(New Runtime.Serialization.Formatters.Binary.BinaryFormatter().Deserialize(F), Net.CookieContainer)
            End Using
        End Function
    End Class
