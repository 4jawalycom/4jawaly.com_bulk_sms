Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks

Namespace API_Example.Models
    Public Class _4jawalyProvider
        Public Property app_key As String
        Public Property app_secret As String
    End Class

    Public Class message
        Public Property text As String
        Public Property numbers As List(Of String)
    End Class

    Public Class Globals
        Public Property number_iso As String
        Public Property sender As String
    End Class

    Public Class SendResult
        Public Property Sent As Boolean
        Public Property Message As String
    End Class

    Public Class SendData
        Public Property messages As List(Of message)
        Public Property globals As Globals
    End Class

    Public Class Account
        Public Property id As Integer
        Public Property account_id As Integer
        Public Property user_id As Integer
        Public Property currency_id As Integer
        Public Property name As String
        Public Property username As String
        Public Property email As String
        Public Property mobile As String
        Public Property name_hash As String
        Public Property username_hash As String
        Public Property email_hash As String
        Public Property mobile_hash As String
        Public Property main_account_id As Object
        Public Property status As Integer
        Public Property is_marketer As Integer
        Public Property created_at As DateTime
        Public Property updated_at As DateTime
        Public Property queue_number As String
    End Class

    Public Class RMessage
        Public Property inserted_numbers As Integer
        Public Property message As Message2
        Public Property err_text As String
    End Class

    Public Class Message2
        Public Property account_id As Integer
        Public Property job_id As String
        Public Property text As String
        Public Property sender_id As Integer
        Public Property sender_name As String
        Public Property encoding As String
        Public Property length As Integer
        Public Property per_message As Integer
        Public Property remaining As Integer
        Public Property messages As Integer
        Public Property send_at As Object
        Public Property send_at_zone As Object
        Public Property updated_at As DateTime
        Public Property created_at As DateTime
        Public Property id As Integer
        Public Property account As Account
    End Class

    Public Class _4jawalyRoot
        Public Property job_id As String
        Public Property messages As List(Of RMessage)
        Public Property code As Integer
        Public Property message As String
    End Class

    Public Class Datum
        Public Property id As Integer
        Public Property sender_name As String
        Public Property is_ad As Integer
        Public Property status As Integer
        Public Property created_at As DateTime
    End Class

    Public Class Items
        Public Property current_page As Integer
        Public Property data As List(Of Datum)
        Public Property first_page_url As String
        Public Property from As Integer
        Public Property last_page As Integer
        Public Property last_page_url As String
        Public Property links As List(Of Link)
        Public Property next_page_url As Object
        Public Property path As String
        Public Property per_page As Integer
        Public Property prev_page_url As Object
        Public Property [to] As Integer
        Public Property total As Integer
    End Class

    Public Class Link
        Public Property url As String
        Public Property label As String
        Public Property active As Boolean
    End Class

    Public Class Sender
        Public Property code As Integer
        Public Property message As String
        Public Property items As Items
    End Class

    Public Class _4JawalyUser
        Public Property name As String
        Public Property email As String
        Public Property mobile As String
        Public Property country_iso As String
        Public Property company_name As String
        Public Property is_marketer As String
        Public Property account_type As String
    End Class

    Public Class Item
        Public Property email As String
        Public Property mobile As String
        Public Property by_account_id As Integer
        Public Property account_id As Integer
        Public Property country_iso As String
        Public Property name As String
        Public Property account_type As String
        Public Property company_name As String
        Public Property company_field_id As Integer
        Public Property is_developer As Integer
        Public Property site_user_id As Object
        Public Property site_username As Object
        Public Property site_groupid As Object
        Public Property site_createdby As Integer
        Public Property is_marketer As String
        Public Property currency_id As Integer
        Public Property odoo_id As String
        Public Property status As Integer
        Public Property source As String
        Public Property is_transfer As Boolean
        Public Property fb_id As Object
        Public Property google_id As Object
        Public Property zoho_id As Object
        Public Property updated_at As DateTime
        Public Property created_at As DateTime
        Public Property id As Integer
        Public Property id_number As String
        Public Property is_first_login As Boolean
        Public Property is_first_login_after_transfer As Boolean
    End Class

    Public Class AddUserRepons
        Public Property code As Integer
        Public Property message As String
        Public Property item As Item
    End Class

    Public Class Network
        Public Property id As Integer
        Public Property image As String
        Public Property image_path As String
        Public Property network_tags As List(Of NetworkTag)
    End Class

    Public Class NetworkTag
        Public Property id As Integer
        Public Property image As String
        Public Property title As String
        Public Property network_id As Integer
        Public Property image_path As String
    End Class

    Public Class Package
        Public Property id As Integer
        Public Property title_ar As String
    End Class

    Public Class Balance
        Public Property code As Integer
        Public Property message As String
        Public Property items As List(Of Item)
        Public Property total_balance As Integer
    End Class

    Public Class Response
        Public Property code As Integer
        Public Property message As String
    End Class
End Namespace
