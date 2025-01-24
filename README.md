# Turbo Dry Oven Web MQTT Client

## Project Description

This project involves the development of a **Web MQTT client** with a graphical user interface (GUI). The client is designed to display specific information about a device called **Turbo Dry**, which is part of the **RPL LINE** process for shoe uppers for the customer **FILATI**.

The application includes a single-page interface that dynamically displays real-time data received via MQTT, such as station and position information, device status, and the RSA company logo.

---

## Features

- **Dynamic Data Display:** Subscribes to MQTT topics to display real-time data from the Turbo Dry oven system.
- **Single-Page Application:** All information is displayed on one clean, user-friendly web page.
- **MQTT Integration:** Handles various data types, including integers, strings, booleans, and images, via MQTT subscriptions.
- **Responsive Design:** Ensures the interface adapts to different screen sizes for an optimal user experience.

---

## Data Displayed

The following data is displayed on the page:

1. **RSA Company Logo:** BMP image.
2. **Current Station Number:** Integer.
3. **Position Information:** Strings from six stations:
   - **Level 2:** Left and Right Position Info.
   - **Level 1:** Left and Right Position Info.
4. **Sole Presence:** Boolean values for Left and Right.

All the data is dynamically updated via MQTT subscriptions.

---

## Technology Stack

- **Frontend:** HTML, CSS, JavaScript.
- **Backend:** C# and MQTTnet client for real-time communication.
- **MQTT Broker:** Mosquitto.
- **Image Handling:** BMP file support.
