   M S B A M L   `   `   `  ����C  ?MasterWF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:MasterWFMasterWF  5   	       MasterWF.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:MasterWF     5   	   -   5          � Title$  
MainWindow��5   	   $	��600��6   $	��800��6)   .��5
      �  � ColumnDefinitions 5   
   �� 5      $��1* �6   �� 5      $��4* �6    � RowDefinitions 5   
   �� 5      $��2* �6   5      �� 5      $��2* �6   5      .+�5   
   �� -   5         ��Name$ tbSqlSource��5      $��1��6'   $��1��64    �� TextWrapping$
 Wrap=�6D    �� AcceptsReturn . 6X   R� 5   
   -   5        $��trvMenu��5      $��0��6"   $��0��6/    R� SelectedValuePath$  ��6Y    � 	Resources 5       �� DataType   MasterWF.Table	 �� ItemsSource
 �� Orientation   MasterWF.HlpTable��%�  (��       5   +   *, +)(�� �     5      *, +)�� 5      "  6+   	 6K   �� *ColumnsList+.R�5      �� $
 
Horizontal=�6!   .+�5      �� ��6$   �� *Name+5      5      �� 5      "  6   .R�5      �� $
 
Horizontal=�6!   .+�5       �� ��6$   �� *Name+�� 5!      $�� (��6$   
��� � �6.   �� 5"      
��� � ���6$   �� *
DataType+6>   �� 5#      $��)��6$   
��� � �6-   5$      5%      5&      5(      � 5)   
   $��0��6   $��1��6    5*      �� 5+      $��1* �6#   �� 5,      $��1* �6#    5.      �� 5/      $��1* �6    .+�51      &   System.Windows.Controls.DataGrid  -   5        $��DataGridSource��51   5   $��0��6   $��0��6(      Columns 52      0  *System.Windows.Controls.DataGridTextColumn  53      ,  &System.Windows.Controls.DataGridColumn   Header$
 Name��6)   1  +System.Windows.Controls.DataGridBoundColumn   Binding 67   �� *Name+  54      $
 Type��6)    67   �� *
DataType+  55      $ Dest Column��6)    6>   �� *MapDestColumn+4  .System.Windows.Controls.DataGridCheckBoxColumn  56      $	 Key��6-    6:   �� *Key+  57      $ Alias��6)    68   �� *	Mapping+  58      $ Source Table��6)    6?   �� *
TableAlias+5=        5L      -   5        $��DataGrid��5L   4   $��1��6   $��0��6'    5M        5N      $
 Name��6)    67   �� *Name+  5O      $
 Type��6)    67   �� *
DataType+  5P      $	 Key��6-    6:   �� *Key+5S      5U      �� 5V   
   -   5         ��Name$ 	textBlock��5V      $
��Left=�6'   $��	10,10,0,0q�6B   $��1��6U   $
��Wrap=�6b   $��	TextBlock��6v   $	��Top=�6�   $	��265��6�   $	��138��6�   5Z      5[      