   M S B A M L   `   `   `  ����D  @SqlHelper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:SqlHelper	SqlHelper  5   	       SqlHelper.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:SqlHelper     5   	   -   5          � Title$  
MainWindow��5   	   $	��800��6   $	��600��6)   .��5
      �  � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $
��Auto �6   �� 5      $��* �6    � RowDefinitions 5   
   �� 5      $��20 �6   �� 5      $��1* �6   �� 5      $��1* �6   �� 5      $��2* �6   .+�5   
   �� $��Right=�6   $��Stretch=�5      $��1��5      $��1��5       �� ResizeBehavior$ PreviousAndNext=�5      $��5��5      ���	#FFBCBCBC6   �� 5   
   -   5        $��	ChBoxChar��5      $��Ignore Chars��6$   �� 5   
   -   5         ��Name$ c1��5      $��0��6   $��1��6-    K� ZIndex$ 1��6:   �� 5    
   -   5         ��Name$ LvScColumns��5       $��0��6)   $��1��69    J� 	AllowDrop .  �� View 5#      �� 5$      .P�5%      �� $
��Name��6%   	 �� DisplayMemberBinding	 63   �� *Name+�� 5&      $
��Type��6%   	 63   �� *
DataType+�� 5'      $��Alias��6%   	 64   �� *	Mapping+�� 5(      $	��Key��6%   	 62   �� *Key+5)      5,      �� 5-   
   -   5        $ 	LvColumns��5-      $��2��6'   $��1��67    .  50      �� 51      .P�52      �� $
��Name��6%   	 63   �� *Name+�� 53      $
��Type��6%   	 63   �� *
DataType+�� 54      $��	Constrain��6%   	 68   �� *	Constrain+�� 55      $��Mapping��6%   	 66   �� *	Mapping+�� 56      $	��Key��6%   
 �� SourceUpdated
 62   �� *Key+57      59      �� 5:   
   -   5         ��Name$ tbSql��5:      $��3��6!   $��2��65    �� TextWrapping$
 Wrap=�6B    �� AcceptsReturn . 6V   �� 5;   
   -   5        $ tbSqlSource��5;      $��0��6'   $��3��67   $
 Wrap=�6D    . 6X   �� 5<   
   -   5        $ 	tbSqlDesc��5<      $��2��6%   $��3��65   $
 Wrap=�6a    . .��6�    � ToolTip 5=      _� 5>       � DataContext 6   ��  �� Path$ PlacementTarget� �� Self �� RelativeSource# Z .��5?      �� .+�5@      �� $
��Bold"�6    $
��Help��62   �� 5A      $1��+F6 - Create Select command based on mapping��6    �� 5B      $+��%F5 - Parse DDL, create select command��6    �� 5C      $��F4 - Map by name��6    �� 5D      $ ��F3 - Create Update command��6    �� 5E      $��F2 - Create inser command��6    5F      5G      .��5H      5I      5J      5K      