# Camel-Courier

[![Alt text](https://img.youtube.com/vi/jHOke2zpl4E/0.jpg)](https://www.youtube.com/watch?v=jHOke2zpl4E&feature=youtu.be "Demo Video")

**[Demo Video](https://www.youtube.com/watch?v=jHOke2zpl4E&feature=youtu.be)**

This project is intended to stream the screen of one device to another on the same LAN. The current configuration is for running the client and server on the same device. Before attempting to run on seperate devices, you must first configure the IP endpoints. The Client project can accept the argument of IPAddress.Any, and the server should have the argument IPAddress.Parse(/*your target ip*/).
