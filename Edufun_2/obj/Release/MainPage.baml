   M S B A M L   `   `   `  ���� C  ?Edufun_2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:Edufun_2Edufun_2      Edufun_2.MainPage   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     (localclr-namespace:Edufun_2     -     N� Title$  MainPage��.��� .+�� 
�������$��0q�.+�� $��50��$	��Top=�$
��Left=�$��50,0,0,0q� � ColumnDefinitions �� $	��100 ��� $	��150 ��� $	��150 ��� $	��130 ��� $	��180 ��� $��50 ��� $
��auto �.+��� -   $��
SaveButton��$��생성하기��$��0��$��20(�$��0,10,0,0q��� -   $��
cb_subject��$��1��$��Center=�$��24(�$��20,0,0,0q� �� SelectedIndex$ 0��.@��� $��과목전체���� $��생명���� $��실험���� -   $��cb_department��$��2��$��Center=�$��24(�$��20,0,0,0q�$ 0��.@��� $��소속전체���� $��직영지사���� $��직영개인���� $��위탁회원���� $��기타1���� $��기타2���� -   $��	cb_search��$��3��$��Center=�$��24(�$��30,6,0,6q�$ 0��$
��Left=�$	��100��.@��� $��이름���� $��	폰번호���� $��	이메일���� -    ��Name$ 	tb_search��$��4��$��0,0,0,0q�$��Center=�$��24(��� -    ��Name$ 	bt_search��$��5��$��검색��$��Center=�$��24(��� -   $ bt_class��$��6��$��수업 정보��$��Center=�$��24(�$��50,0,0,0q��� -	   $��
myListView��$��
13,60,27,0q�$��Center=�$	��Top=�$
��Left=� �� View �� .P��� $	��idx�� �� Width$ 50�� �� CellTemplate �� .R��� $
��Wrap=�$��Center=�$��Center=�$��20(����� *idx+�� $
��Name��$	 100�� �� .R��� $
��Wrap=�$��Center=�$��Center=�$��20(����� *Name+�� $��Phone��$	 200�� �� .R��� $
��Wrap=�$��Center=�$��Center=�$��20(����� *Phone+�� $��Email��$	 200�� �� .R��� $
��Wrap=�$��Center=�$��Center=�$��20(����� *Email+�� $��Subject��$	 120�� �� .R��� $
��Wrap=�$��Center=�$��Center=�$��20(����� *	Subject+�� $��Department1��$	 140�� �� .R��� $
��Wrap=�$��Center=�$��Center=�$��20(����� *Department1+�� $��Department2��$	 140�� �� .R��� $
��Wrap=�$��Center=�$��Center=�$��20(����� *Department2+�� -
   $��	btnExport��$��50��$	��100��$��내보내기��$��24(�$��Bottom=�$��	0,0,50,20q�$��Right=�