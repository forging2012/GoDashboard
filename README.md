# GoDashboard

Information radiator for the Go Continuous Delivery Software by Thoughtworks (http://go.cd).

## Description
The GoDashboard is a ASP.NET C# web application which is designed to run full screen on a monitor to give an at-a-glance view of a selection of pipelines and their current build statuses. The dashboard can show all pipelines in the system, or be configured to show just a subset.

## Disclaimer
This dashboard is an internal tool at The Laterooms Group which we have released to the community. If you find any problems or have any feature suggestions, please log an issue in Github, or even better - fork it and submit a pull request.

### Example Screenshot
Dashboard configured to show 3 groups of pipelines, 2 with Group headers.

![Screenshot](https://raw.githubusercontent.com/LateRoomsGroup/GoDashboard/master/screenshots/screenshot1.png)

## Configuration

1. Modify `GoDashboard.Web\AppSettings.config` to set your Go username and password and set the URL to the CC Tray XML end point. It is advisable to create a user account in Go that has readonly permissions to all pipelines and use this account for the dashboard.
2. Modify `GoDashboard.Web\Xml\Profiles.xml` to configure your dashboard profiles. See [Profiles.xml](#profilesxml) below for full details.
3. Compile and deploy to an IIS server. (Tested on IIS 7.5+)
4. To load a specific profile (defined in the XML), append `?profileName=<profilename>` to the address.

<a name="profilesxml"></a>
### Profiles.xml

The `GoDashboard.Web\Xml\Profiles.xml` file describes the dashboard profiles. Each profile can contain multiple groups, each with a number of piplines in. Information about the last build can be hidden by adding `hideBuildInfo="true"` attribute to a `<Pipeline>`.

#### Example XML

This is the XML for the screen shot above.
```xml
<Profile name="My Dashboard">
    <WhiteList>
        <Group name="Group 1" showName="true">
            <Pipeline>Website-CI</Pipeline>
            <Pipeline>Website-QA</Pipeline>
        </Group>
        <Group name="Group 2" showName="true">
            <Pipeline  hideBuildInfo="true">API-CI</Pipeline>
            <Pipeline hideBuildInfo="true">API-QA</Pipeline>
        </Group>
        <Group name="Group 3" showName="false">
            <Pipeline>Services-CI</Pipeline>
            <Pipeline>Services-QA</Pipeline>
            <Pipeline>Services-Live-Deploy</Pipeline>
        </Group>
    </WhiteList>
    <Statuses>
        <Passed/>
        <Failed/>
        <Building/>
    </Statuses>
</Profile>
```

#### XSD Schema
```xml
<xs:schema>
    <xs:element name="Profiles">
        <xs:complexType>
            <xs:sequence>
                <xs:element name="Profile" maxOccurs="unbounded" minOccurs="1">
                    <xs:complexType>
                        <xs:sequence>
                            <xs:element name="WhiteList">
                                <xs:complexType>
                                    <xs:sequence>
                                        <xs:element type="xs:string" name="Pipeline" maxOccurs="unbounded" minOccurs="1"/>
                                        <xs:element name="Group" maxOccurs="unbounded" minOccurs="0">
                                            <xs:complexType mixed="true">
                                                <xs:sequence>
                                                    <xs:element name="Pipeline" maxOccurs="unbounded" minOccurs="1">
                                                        <xs:complexType>
                                                            <xs:simpleContent>
                                                                <xs:extension base="xs:string">
                                                                    <xs:attribute type="xs:string" name="alias" use="optional"/>
                                                                    <xs:attribute type="xs:string" name="hideBuildInfo" use="optional"/>
                                                                </xs:extension>
                                                            </xs:simpleContent>
                                                        </xs:complexType>
                                                    </xs:element>
                                                </xs:sequence>
                                                <xs:attribute type="xs:string" name="name" use="optional"/>
                                                <xs:attribute type="xs:string" name="showName" use="optional"/>
                                            </xs:complexType>
                                        </xs:element>
                                    </xs:sequence>
                                </xs:complexType>
                            </xs:element>
                            <xs:element name="Statuses">
                                <xs:complexType>
                                    <xs:sequence>
                                        <xs:element type="xs:string" name="Passed" minOccurs="0"/>
                                        <xs:element type="xs:string" name="Failed" minOccurs="0"/>
                                        <xs:element type="xs:string" name="Building" minOccurs="0"/>
                                    </xs:sequence>
                                </xs:complexType>
                            </xs:element>
                            <xs:element type="xs:string" name="ShowPassedCount" minOccurs="0"/>
                        </xs:sequence>
                        <xs:attribute type="xs:string" name="name" use="optional"/>
                    </xs:complexType>
                </xs:element>
            </xs:sequence>
        </xs:complexType>
    </xs:element>
</xs:schema>
```

