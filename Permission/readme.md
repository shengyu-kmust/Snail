# 功能
* 权限的默认实现，实现Core里的Permission模块
1、用户注册、登录、管理功能
2、角色自定义，管理
3、用户角色授权
4、角色资源授权
5、组织架构管理和组织架构的用户管理
6、所有资源（指要做权限控制的资源）管理
7、判断用户是否有某资源的访问权限

## 如何用
### 实现权限的方式有如下两种
1、dbContext继承PermissionDatabaseContext，并调用AddDefaultPermission
2、实现IPermission和IPermissionStore接口并注册，调用AddPermissionCore

### 怎么将方法纳入到权限控制 
* 在controller或是action上加入Authorize(Policy = PermissionConstant.PermissionAuthorizePolicy)，让方法进入到授权处理
* 在action上以ResourceAttribute进行标识，这样才能在InitResource接口里将方法做为资源初始化到表里



## 脚本
* 初始化数据库
~~~
insert into Role(Id,CreateTime,UpdateTime,IsDeleted,Name)
select '1',GETDATE(),GETDATE(),0,'superAdmin'
insert into UserRole(Id,CreateTime,UpdateTime,IsDeleted,UserId,RoleId)
select '1',GETDATE(),GETDATE(),0,'1','1'
insert into Users(Id,CreateTime,UpdateTime,IsDeleted,Account,Name,Pwd,Gender)
select '1',GETDATE(),GETDATE(),0,'zhoujing','周晶','E10ADC3949BA59ABBE56E057F20F883E',1--pwd为123456的md5
~~~