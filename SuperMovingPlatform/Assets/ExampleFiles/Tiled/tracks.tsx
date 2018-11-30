<?xml version="1.0" encoding="UTF-8"?>
<tileset name="Tracks" tilewidth="16" tileheight="16" tilecount="6" columns="3">
 <properties>
  <property name="unity:layer" value="Track"/>
 </properties>
 <image source="tracks-ts.png" width="48" height="32"/>
 <tile id="0">
  <objectgroup draworder="index">
   <object id="1" x="8" y="16">
    <polyline points="0,0 0,-8 8,-8"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="1">
  <objectgroup draworder="index">
   <object id="1" x="0" y="8">
    <polyline points="0,0 8,0 8,8"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="2">
  <objectgroup draworder="index">
   <object id="1" x="0" y="8">
    <polyline points="0,0 16,0"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="3">
  <objectgroup draworder="index">
   <object id="1" x="8" y="0">
    <polyline points="0,0 0,8 8,8"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="4">
  <objectgroup draworder="index">
   <object id="1" x="0" y="8">
    <polyline points="0,0 8,0 8,-8"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="5">
  <objectgroup draworder="index">
   <object id="1" x="8" y="0">
    <polyline points="0,0 0,16"/>
   </object>
  </objectgroup>
 </tile>
</tileset>
