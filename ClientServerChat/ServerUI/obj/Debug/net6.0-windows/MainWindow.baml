   M S B A M L   `   `   `  ����C  ?ServerUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:ServerUIServerUI  5   	       ServerUI.MainWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:ServerUI     5   	     � Title$  
MainWindow��5   	   $	��450��6   $	��800��6)   .��5	      �  � ColumnDefinitions 5
   
   �� 5      $
��513* �6   �� 5      $
��287* �6    � RowDefinitions 5   
   �� 5      $	��19* �6   �� 5      $	��43* �6   .+�5   
   �� -   5         ��Name$ ConnectedClientsListBox��5      $��2��63   $��133,26,10,0q�6D   $��1��6Y   �� 5   
   -   5         ��Name$ TextBoxServerBoxLog��5      $��Center=�6/   $��0,26,0,0q�6L    �� TextWrapping$
 Wrap=�6^   $�� ��6r   $	��Top=�6z   $	��510��6�   $	��408��6�   $��2��6�   5      5      