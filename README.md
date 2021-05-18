# UnseonChartLib

This Library can use on environment .NET5.0(WPF)<br/>
It is not completed project.<br/>
it's launch target date is 2021-05-21 (Beta, Only including Simple Line Chart)<br/>

![image](https://user-images.githubusercontent.com/35219280/118642168-fd64d580-b815-11eb-8d04-8407df9cb1bc.png)

<br/>
<br/>
<br/>

# Chart Tutorial

1. Add UserChartLib (DLL File or by UserChartLib Project)

![image](https://user-images.githubusercontent.com/35219280/118647344-33a55380-b81c-11eb-8e6f-65f1050652ba.png)

2. You can find chartElements by user control section of toolbox <br/>

![image](https://user-images.githubusercontent.com/35219280/118647689-94349080-b81c-11eb-9468-28b1e9d5296e.png)

3. Also You can create by write system for xaml<br/>

![image](https://user-images.githubusercontent.com/35219280/118647863-cb0aa680-b81c-11eb-86be-849fed8e0377.png)

3. Initialize chart Option & columns of dataTable<br/>

    - you should control chart between EnterLock() and ExitLock() 
    
![image](https://user-images.githubusercontent.com/35219280/118647895-d4940e80-b81c-11eb-893c-8dba13555fdd.png)

4. You can data live streaming by other threads<br/>

    - First. start your work thread.

![image](https://user-images.githubusercontent.com/35219280/118648344-40767700-b81d-11eb-8a5f-09d8fa03fc2a.png)

    - Seconds. you can add rows everyTime & any thread 
    
![image](https://user-images.githubusercontent.com/35219280/118648443-5d12af00-b81d-11eb-9caf-eccadb919436.png)



