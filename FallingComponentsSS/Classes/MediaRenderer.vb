Public Class MediaRenderer

   ' Chooses the right drawing class for the item's Kind
   Public Shared Sub Draw(g As Graphics, item As MediaItem, tint As Color)
      Select Case item.Kind
         Case MediaType.FDD
            FDD.Draw(g, item, tint)
         Case MediaType.CDROM
            CDROM.Draw(g, item, tint)
         Case MediaType.CassetteTape
            CassetteTape.Draw(g, item, tint)
         Case MediaType.HardDisk
            HardDisk.Draw(g, item, tint)
         Case MediaType.CPU
            CPU.Draw(g, item, tint)
         Case MediaType.Memory
            Memory.Draw(g, item, tint)
         Case Else
            FDD.Draw(g, item, tint)
      End Select
   End Sub

End Class