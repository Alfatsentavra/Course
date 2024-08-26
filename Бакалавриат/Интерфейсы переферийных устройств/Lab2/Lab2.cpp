#include "pch.h"
#include <windows.h>    
#include <iostream>
#include <lusb0_usb.h>
#include <conio.h>
using namespace std;

static void print_endpoint(struct usb_endpoint_descriptor* endpoint)
{
	printf("\n ENDPOINT DESCRIPTOR\n\n");
	printf("			bEndpointAddress: %02xh\n", endpoint->bEndpointAddress);
	printf("			bmAttributes:  %02xh\n", endpoint->bmAttributes);
	printf("			wMaxPacketSize: %d\n", endpoint->wMaxPacketSize);
	printf("			bInterval: %d\n", endpoint->	bInterval);
	printf("			bRefresh: %d\n", endpoint->bRefresh);
	printf("			bSynchAddress: %d\n", endpoint->bSynchAddress);
}
static void print_interface(struct usb_interface_descriptor* altsetting)
{
	printf("\n INTERFACE DESCRIPTOR\n\n");
	printf("		bInterfaceNumber: %d\n", altsetting->bInterfaceNumber);
	printf("		bAlternateSetting:  %d\n", altsetting->bAlternateSetting);
	printf("		bNumEndpoints: %d\n", altsetting->bNumEndpoints);
	printf("		bInterfaceClass: %d\n", altsetting->bInterfaceClass);
	printf("		bInterfaceSubClass: %d\n", altsetting->bInterfaceSubClass);
	printf("		bInterfaceProtocol: %d\n", altsetting->bInterfaceProtocol);
	printf("		iInterface: %d\n", altsetting->iInterface);
	for (int k = 0; k < (int)altsetting->bNumEndpoints; k++)
		print_endpoint(&altsetting->endpoint[k]);
	printf("########################################");
}
static void print_configuration(struct usb_config_descriptor* config)
{
	printf("\n CONFIG DESCRIPTOR\n\n");
	printf("	wTotalLength: %d\n", config->wTotalLength);
	printf("	bNumInterfaces:  %d\n", config->bNumInterfaces);
	printf("	bConfigurationValue: %d\n", config->bConfigurationValue);
	printf("	iConfiguration: %d\n", config->iConfiguration);
	printf("	bmAttributes: %02xh\n", config->bmAttributes);
	printf("	MaxPower: %d\n", config->MaxPower);
	print_interface(config->interface->altsetting);
}
static int print_device(struct usb_device* dev)
{
	usb_dev_handle* udev;
	char string[256];
	int ret;
	udev = usb_open(dev);
	if (udev == 0)
		printf("Couldn't open usb device : %s", usb_strerror());
	else
	{
		ret = usb_get_string_simple(udev, dev->descriptor.iManufacturer, string, 256);
		//printf("--Manufacturer: %s \n", string);
		printf("--VID: %04X\n", dev->descriptor.idVendor);
		printf("--PID: %04X \n", dev->descriptor.idProduct);
		ret = usb_get_string_simple(udev, dev->descriptor.iProduct, string, 256);
		printf("--iProduct: %s\n", string);
		ret = usb_get_string_simple(udev, dev->descriptor.iSerialNumber, string,
			sizeof(string));
		printf("--Serial Number: %s\n\n", string);
		printf("DEVICE DESCRIPTOR\n\n");
		printf(" bLenght: %d\n", dev->descriptor.bLength);
		printf(" bDescriptorType: %02xh\n", dev->descriptor.bDescriptorType);
		printf(" bDeviceClass: %02xh\n", dev->descriptor.bDeviceClass);
		printf(" bDeviceSubClass: %02xh\n", dev->descriptor.bDeviceSubClass);
		printf(" bDeviceProtocol: %02xh\n", dev->descriptor.bDeviceProtocol);
		printf(" bMaxPacketSize0: %02xh\n", dev->descriptor.bMaxPacketSize0);
		printf(" idVendor: %02xh\n", dev->descriptor.idVendor);
		printf(" idProduct: %02xh\n", dev->descriptor.idProduct);
		printf(" bcdDevice: %02xh\n", dev->descriptor.bcdDevice);
		print_configuration(&dev->config[0]);
		printf("\n");
		return 0;
	}
	return 0;
}
int main(int argc, char* argv[])
{
	struct usb_bus* bus;
	usb_init();
	while (true) {
		usb_find_busses();
		if (usb_find_devices() != 0) {
			system("cls");
			for (bus = usb_busses; bus; bus = bus->next) {
				struct usb_device* dev;
				for (dev = bus->devices; dev; dev = dev->next)
					print_device(dev);
				Sleep(1000);
			}
		}
	}
	getchar();
	return 0;
}
