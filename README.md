# Edufun Project (For My Daddy😘)
## .NET Framework 4.7.2 WPF 강사 DB 프로그램
### 페이지 구성
* MainPage : 강사 정보를 포함한 리스트 페이지.
* ClassPage : 분기별 강사의 학생수 리스트 페이지.
* DetailPage : 강사 별 세부사항 페이지. 년도 별, 분기 별, 요일 별, 교시 별 학생수 표 포함. (강사의 수업 별 학생수 추가 가능)
* EditPage : 강사 별 세부사항 수정 가능. (강사의 수업 별 학생수 수정,추가 가능)
* CreatePage : 강사 추가 페이지.

### 데이터 구성(SQLite 사용)
* Instructor 
 - ID
 - Name
 - Phone
 - Email
 - Address
 - Subject
 - Department1
 - Department2
 - ShipAddress1
 - ShipMethod1
 - ShipAddress2
 - ShipMethod2
 - Remark
* Class
 - ID
 - Instructor_ID
 - Year
 - Quarter
 - Day
 - Time
 - Student_count
