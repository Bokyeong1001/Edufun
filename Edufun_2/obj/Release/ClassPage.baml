   M S B A M L   `   `   `  ���� C  ?Edufun_2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:Edufun_2Edufun_2      Edufun_2.ClassPage   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     (localclr-namespace:Edufun_2     -     N� Title$  	ClassPage��.��� 
�������.+�� $��50��$	��Top=�$��30,0,0,0q� � RowDefinitions �� �� $��4* � � ColumnDefinitions �� $	��100 ��� $	��100 ��� $	��140 ��� $	��130 ��� $	��150 ��� $��60 �.+��� -   $��bt_back��$��0��$��뒤로가기��$��24(�$��2���� -   $��cb_year��$��1��$��Center=�$��24(�$��20,6,0,6q� �� SelectedIndex$ 0��$��2��.@��� $
��2021���� $
��2020���� $
��2019���� -   $��
cb_quarter��$��2��$��Center=�$��24(�$��20,6,0,6q�$ 0��$��2��.@��� $��분기전체���� $��1분기���� $��2분기���� $��3분기���� $��4분기���� -   $��	cb_search��$��3��$��Center=�$��24(�$��30,6,0,6q�$ 0��$
��Left=�$	��100��$��2��.@��� $��이름���� $��	폰번호���� $��	이메일���� -    ��Name$ 	tb_search��$��4��$	��0,8q�$��Center=�$��24(�$��2���� -    ��Name$ 	bt_search��$��5��$��검색��$��Center=�$��24(�$	��0,7q�$��2���� -   $��classListView��$��
13,60,27,0q�$��Center=�$	��Top=�$
��Left=� �� View �� .P��� $	��idx�� �� Width$ 50�� �� CellTemplate �� .R��� $
��Wrap=�$��Center=�$��Center=�$��20(����� *idx+�� $
��Name��$	 100�� �� .R��� $
��Wrap=�$��Center=�$��Center=�$��20(����� *Name+�� $��Phone��$	 150�� �� .R��� $
��Wrap=�$��Center=�$��Center=�$��20(����� *Phone+�� $��Subject��$	 100�� �� .R��� $
��Wrap=�$��Center=�$��Center=�$��20(����� *	Subject+�� $��Quarter��$ 80�� �� .R��� $
��Wrap=�$��Center=�$��Center=�$��20(����� *	Quarter+�� $��Department1��$	 150�� �� .R��� $
��Wrap=�$��Center=�$��Center=�$��20(����� *Department1+�� $��Department2��$	 150�� �� .R��� $
��Wrap=�$��Center=�$��Center=�$��20(����� *Department2+�� $��Student_count��$	 100�� �� .R��� $
��Wrap=�$��Center=�$��Center=�$��20(����� *Student_count+�� -	   $��	btnExport��$��50��$	��100��$��내보내기��$��24(�$��Bottom=�$��	0,0,50,20q�$��Right=�