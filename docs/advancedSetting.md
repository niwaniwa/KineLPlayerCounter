
<h1> Base settings </h1>

# Overview
Can be configured for area functions

!> This document is under translation.

## What is area function?
The number of people in a collider set in any range can be displayed.

### How to setting
Enable the area feature by setting in PlayerCounter.prefab.
By setting the BoxCollider in PlayerCountArea.prefab to any area, you can display the players that have entered that collider.
Multiple Areas can be specified.

## PlayerCounter.prefab
---
|Name|Description|type|
|--|--|:--:|
|area|Enable area function||
|Areas|Specify the area you want to count.|PlayerCountArea|

## PlayerCountArea.prefab
---
|Name|Description|type|
|--|--|:--:|
|Debug|Displays debug messages in the console.|boolean|