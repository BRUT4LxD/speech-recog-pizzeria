   M S B A M L   `   `   `  ����C  ?Pizzeria, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:PizzeriaPizzeria  5   	       Pizzeria.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:Pizzeria     5   	   -   5          �Name$  PizzaNum��5   	    � Title$ 
MainWindow��5   	   $	��450��6   $��729.449��6)   $��32(�69   $��Vladimir Script)�6G   .��5	      � $��0,0,0,0q�6   ��5
   
   �� 5       �� ImageSource$ 	pizza.jpg��6    j� Stretch$ UniformToFill=�61    �� Opacity$	 0.3Q�6I    � ColumnDefinitions 5   
   �� 5      $��20 �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $��20 �6    � RowDefinitions 5   
   �� 5      $	��100 �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $��20 �6   .+�5   
   �� $��0��6   $��0��6!   $��Pizzeria��61   $��4��6A   $��Center=�5      $��20q�61   $��64(�6=   $��Vivaldi)�6K   �� 5   
   $��1��6   $��1��6!   $��Pizza:��61   $��10q�6?   �� 5    
   $��2��6   $��1��6!   $��Ciasto: ��61   $��10q�6A   �� 5!   
   $��3��6   $��1��6!   $	��Sos��61   $��10q�6<   �� 5"   
   $��4��6   $��1��6!   $��Do zapłaty:��61   $��10q�6D   �� 5#   
   $��5��6   $��3��6!   $!��Najlepsza pizza w galaktyce��61   $��23(�6T   $��Viner Hand ITC)�6b   �� 5$   
   -   5         ��Name$ pizzaNumber��5$   /   $��1��6   $��2��6   $	��200��6D   $��10q�6P   �� 5%   
   -   5        $ 	pizzaCake��5%   /   $��2��6   $��2��6   $	��200��6B   $��10q�6N   �� 5&   
   -   5        $ pizzaDip��5&   /   $��3��6   $��2��6   $	��200��6A   $��10q�6M   �� 5'   
   -   5        $ 
pizzaPrice��5'   /   $��4��6   $��2��6   $	��200��6C   $��10q�6O   5(      5)      