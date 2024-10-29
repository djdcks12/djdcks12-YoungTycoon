# 영타이쿤 프로젝트

## 개요

간단한 풀링,SingleTone 패턴, MVC 패턴, Observer 패턴, Unitask 및 Unirx를 이용한 Timer가 적용된 프로젝트
##
## 기본 클래스 소개

[SingletoneClass](https://github.com/djdcks12/djdcks12-YoungTycoon/blob/main/youngYycoon/Assets/Script/SingletoneClass.cs)

싱클톤 클래스들의 부모가 되는 클래스이며 본 프로젝트에서는 컨트롤러들을 관리하는 ControllerManager와 TimeManager가 사용하고있다.
##
[TimeManager](https://github.com/djdcks12/djdcks12-YoungTycoon/blob/main/youngYycoon/Assets/Script/TimeManager.cs)

SingletoneClass를 상속받은 클래스이며 UniRx기반의 타이머 기능을 지원한다. 외부 클래스에서 접근하여 RegisterAction, UnregisterAction등으로 원하는 인터벌에 액션을 등록 해제 할수 있다.
##

[ControllerManager](https://github.com/djdcks12/djdcks12-YoungTycoon/blob/main/youngYycoon/Assets/Script/Controller.cs)

옵저버 패턴을 구현하기위해 만들어진 클래스. SingletoneClass를 상속받았으며 Controller들을 관리한다. 각각의 Controller들은 서로의 존재를 모른채 ControllerManager를 통해 SendMessage로 정보를 주고받는다.
##

[GameManager](https://github.com/djdcks12/djdcks12-YoungTycoon/blob/main/youngYycoon/Assets/Script/GameManager.cs)

게임의 시작으로 Start에서 MainController로 InitMainParm를 담은 Message를 Sned하면서 시작된다. 
##

[Model](https://github.com/djdcks12/djdcks12-YoungTycoon/blob/main/youngYycoon/Assets/Script/Model.cs)
[View](https://github.com/djdcks12/djdcks12-YoungTycoon/blob/main/youngYycoon/Assets/Script/Viwer.cs)
[Controller](https://github.com/djdcks12/djdcks12-YoungTycoon/blob/main/youngYycoon/Assets/Script/Controller.cs)

각각 모델 뷰 컨트롤러의 기본 클래스로 오직 컨트롤러에서만 view와 model에 접근할수 있다.

