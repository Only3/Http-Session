# Http-Session
HttpWebRequest With Save And Load Session (Cookies)

Dim Hs As New HttpSession

Hs.Referer = "http://www.facebook.com"

Dim S = Hs.GET("http://www.facebook.com")

Dim D = String.Format("email={0}&pass={1}&login=Login", "user@gmail.com", "pass")

S = Hs.POST("https://www.facebook.com/login.php?login_attempt=1", D)

'For Each c As Net.Cookie In Hs.Cookies.GetCookies(New Uri("http://www.facebook.com"))

'    MsgBox(c.ToString)

'Next

Hs.SaveSession()

'For Each c As Net.Cookie In Hs.LoadSession.GetCookies(New Uri("http://www.facebook.com"))

'    MsgBox(c.ToString)

'Next
