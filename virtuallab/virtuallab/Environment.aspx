<%@ Page Title="进行实验" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Environment.aspx.cs" Inherits="virtuallab.Environment" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:MultiView ID="EnvironmentView" runat="server" ActiveViewIndex="0">
        <asp:View ID="CodeView" runat="server">
            <div class="row" style="position: relative; margin-top: 20px; margin-bottom: 20px;">
                <div class="col-md-2">
                    <asp:Button runat="server" OnClick="ReloadCode" Text="重新加载模板代码" CssClass="btn btn-default form-control" />
                </div>
                <div class="col-md-6">
                    <asp:Label ID="lbGeneral" runat="server" Text=""></asp:Label>
                </div>
                <div class="col-md-2">
                    <asp:Button runat="server" OnClick="CodeComplie" Text="提交编译" CssClass="btn btn-default form-control" />
                </div>
                <div class="col-md-2">
                    <asp:Button runat="server" OnClick="CodeUpload" Text="上传程序" CssClass="btn btn-default form-control" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-8" style="height: 800px; padding-right: 0px; border-style: solid; border-width: thin;">
                    <textarea id="code_text" class="form-control">
#include <linux/kernel.h>
#include <linux/module.h>
#include <linux/i2c.h>
#include <linux/input.h>
#include <linux/delay.h>
#include <linux/slab.h>
#include <linux/interrupt.h>
#include <linux/irq.h>
#include <linux/gpio.h>
#include <linux/fs.h>
#include <linux/cdev.h>
#include <linux/platform_device.h>
#include <linux/module.h>
#include <linux/cdev.h>
#include <linux/fs.h>
#include <linux/poll.h>
#include <linux/sched.h>

#define ZLG7290_NAME		"zlg7290"
#define ZLG7290_LED_NAME	"zlg7290_led"
#define REG_SYSTEM		0x00
#define REG_KEY_VAL		0x01
#define REG_REPEAT_CNT	0x02
#define REG_FUNC_KEY	0x03
#define REG_CMD_BUF0	0x07
#define REG_CMD_BUF1	0x08
#define REG_FLASH_ONOFF	0x0C
#define REG_SCAN_NUM	0x0D
#define REG_DP_RAM0		0x10
#define REG_DP_RAM1		0x11
#define REG_DP_RAM2		0x12
#define REG_DP_RAM3		0x13
#define REG_DP_RAM4		0x14
#define REG_DP_RAM5		0x15
#define REG_DP_RAM6		0x16
#define REG_DP_RAM7		0x17
#define ZLG7290_LED_MAJOR	800
#define ZLG7290_LED_MINOR	0
#define ZLG7290_LED_DEVICES	1
#define WRITE_DPRAM _IO('Z', 0)
/*****************************************************************
*定义数码管的机构体，封装了I2C client和字符设备描述cdev
******************************************************************/
struct zlg7290
{
	struct i2c_client *client;
	
	struct input_dev *input;
	struct delayed_work work;
	unsigned long delay;
	
	struct cdev cdev;
};

struct zlg7290 *ptr_zlg7290;

unsigned int key_value[65] = {
	0,
	1,  2,  3,  4,  5,  6,  7,  8,
	9,  10, 11, 12, 13, 14, 15, 16,
	17, 18, 19, 20, 21, 22, 23, 24,
	25, 26, 27, 28, 29, 30, 31, 32,
	33, 34, 35, 36, 37, 38, 39, 40,
	41, 42, 43, 44, 45, 46, 47, 48,
	49, 50, 51, 52, 53, 54, 55, 56,
	57, 58, 59, 60, 61, 62, 63, 64,
}; 
/******************************************************************************************/
函数名称：zlg7290_hw_write
函数功能：将buf中的数据写入到指定的数码管中
/******************************************************************************************/
static int zlg7290_hw_write(struct zlg7290 *zlg7290,  int len, size_t *retlen, char *buf)
{
	struct i2c_client *client = zlg7290->client;
	int ret;

	struct i2c_msg msg[] = {
		{ client->addr, 0, len, buf},
	};

	ret =i2c_transfer(client->adapter, msg, 1);
	if (ret < 0) 
	{
		dev_err(&client->dev, "i2c write error!\n");
		return -EIO;
	}

	*retlen = len;
	return 0;
}
/******************************************************************************************/
函数名称：zlg7290_hw_read
函数功能：将当前数码管的值读到buf中
/******************************************************************************************/
static int zlg7290_hw_read(struct zlg7290 *zlg7290, int len, size_t *retlen, char *buf)
{
	struct i2c_client *client = zlg7290->client;
	int ret;
//仿照zlg7290_hw_write()函数，将zlg7290_hw_read()函数补充完整。
	struct i2c_msg msg[] =                  
                           				
                                            ;

	ret =                                        ;
	if (ret < 0) 
	{
		dev_err(&client->dev, "i2c read error!\n");
		return -EIO;
	}

	*retlen = len;
	return 0;
}
//数码管led 打开函数
static int zlg7290_led_open(struct inode *inode, struct file *file)
{
	return 0;
}
//数码管led 释放函数
static int zlg7290_led_release(struct inode *inode, struct file *file)
{
	return 0;
}

static void zlg7290_kpad_work(struct work_struct *work) 
{
	struct zlg7290 *zlg7290 = container_of(work, struct zlg7290, work.work);
	unsigned char val = 0;
	size_t len;

	val = REG_SYSTEM;
	zlg7290_hw_read(zlg7290, 1, &len, &val);
	if(val & 0x1) {
		val = REG_KEY_VAL;
		zlg7290_hw_read(zlg7290, 1, &len, &val);

		if (val == 0) {
			val = REG_FUNC_KEY;
			zlg7290_hw_read(zlg7290, 1, &len, &val);
			if (val == 0 || val == 0xFF)
				goto rework;
		}

		if (val > 56) {
			switch (val) {
				case 0xFE: val = 57; break;
				case 0xFD: val = 58; break;
				case 0xFB: val = 59; break;
				case 0xF7: val = 60; break;
				case 0xEF: val = 61; break;
				case 0xDF: val = 62; break;
				case 0xBF: val = 63; break;
				case 0x7F: val = 64; break;
			}
		}

		input_report_key(zlg7290->input, key_value[val], 1);
		input_report_key(zlg7290->input, key_value[val], 0);
		input_sync(zlg7290->input);
	} 
	return;
	
rework:
	schedule_delayed_work(&zlg7290->work, zlg7290->delay);
}
/******************************************************************************************/
函数名称：zlg7290_led_ioctl
函数功能：用户态调用，将数据写入数码管
函数输入：
filp：设备指针
cmd：对设备的控制命令，一般为WRITE_DPRAM
arg：写入到数码管的数据，无符号整形
用户态编程主要通过ioctl函数来对数码管进行操作。
/******************************************************************************************/
static long 
zlg7290_led_ioctl(struct file *filp, unsigned int cmd, unsigned long arg)
{
	unsigned char data_buf[8] = {0};
	unsigned char write_val[2] = {0};
	ssize_t len = 0;
	int i = 0;
	switch(cmd){
		case WRITE_DPRAM:
//将输入的字符拷贝至data_buf
			if(copy_from_user(data_buf, (void *)arg, 8))
				return -EFAULT;
			//将8位数码管的每一位写入
			for(i = 0; i < 8; i++)
			{
//对write_val进行赋值，第一个字符代表数码管的位数，第二个字符是该位数码管的值，数码管首位的地址为 REG_DP_RAM0
				                         ;
				                         ;
//调用zlg7290_hw_write函数将val写入到数码管中
				                          ;
//休眠1ms
					                ;
			} 
			break;
		default:
			dev_err(&ptr_zlg7290->client->dev, "unsupported command!\n");
			break;
	}
	return 0;
}
static struct file_operations zlg7290_led_fops = {
	.owner = THIS_MODULE,
	.open = zlg7290_led_open,
	.release = zlg7290_led_release,
	.unlocked_ioctl = zlg7290_led_ioctl,
};
/******************************************************************************************/

/******************************************************************************************/
static int register_zlg7290_led(struct zlg7290 *zlg7290) 
{
	struct cdev *zlg7290_cdev;
	int ret;
	dev_t devid;

	devid = MKDEV(ZLG7290_LED_MAJOR, ZLG7290_LED_MINOR);
	ret = register_chrdev_region(devid, ZLG7290_LED_DEVICES, ZLG7290_LED_NAME);
	if (ret < 0) {
		dev_err(&zlg7290->client->dev, "register chrdev fail!\n");
		return ret;
	}

	zlg7290_cdev = &zlg7290->cdev;
	cdev_init(zlg7290_cdev, &zlg7290_led_fops);
	zlg7290_cdev->owner = THIS_MODULE;
	ret = cdev_add(zlg7290_cdev, devid, 1);
	if (ret < 0) {
		dev_err(&zlg7290->client->dev, "cdev add fail!\n");
		goto err_unreg_chrdev;
	}

	return 0;

err_unreg_chrdev:
	unregister_chrdev_region(devid, ZLG7290_LED_DEVICES);
	return ret;
}

static int unregister_zlg7290_led(struct zlg7290 *zlg7290) 
{
	cdev_del(&zlg7290->cdev);
	
	unregister_chrdev_region(MKDEV(ZLG7290_LED_MAJOR, ZLG7290_LED_MINOR), ZLG7290_LED_DEVICES);
	
	return 0;
}

irqreturn_t zlg7290_kpad_irq(int irq, void *handle)
{
	struct zlg7290 *zlg7290 = handle;
	
	schedule_delayed_work(&zlg7290->work, zlg7290->delay);
	
	return IRQ_HANDLED;
}

static int 
zlg7290_probe(struct i2c_client *client, const struct i2c_device_id *id)
{
	struct input_dev *input_dev;
	int ret = 0;
	
	if (!i2c_check_functionality(client->adapter, I2C_FUNC_SMBUS_BYTE)) {
		dev_err(&client->dev, "%s adapter not supported\n",
			dev_driver_string(&client->adapter->dev));
		return -ENODEV;
	}
	
	ptr_zlg7290 = kzalloc(sizeof(struct zlg7290), GFP_KERNEL);
	input_dev = input_allocate_device();
	if (!ptr_zlg7290 || !input_dev) {
		ret = -ENOMEM;
		goto err_free_mem;
	}
	
	input_dev->name = client->name;
	input_dev->phys = "zlg7290-keys/input0";
	input_dev->dev.parent = &client->dev;
	input_dev->id.bustype = BUS_I2C;
	input_dev->id.vendor = 0x0001;
	input_dev->id.product = 0x0001;
	input_dev->id.version = 0x0001;
	input_dev->evbit[0] = BIT_MASK(EV_KEY);
	
	for(ret = 1; ret <= 64; ret++) {
		input_dev->keybit[BIT_WORD(key_value[ret])] |= BIT_MASK(key_value[ret]);
	}

	ret = input_register_device(input_dev);
	if (ret) {
		dev_err(&client->dev, "unable to register input device\n");
		goto err_unreg_dev;
	}

	ptr_zlg7290->client = client;
	ptr_zlg7290->input = input_dev;
	i2c_set_clientdata(client, ptr_zlg7290);
	
	ret = request_threaded_irq(client->irq, NULL, zlg7290_kpad_irq, 
					IRQF_TRIGGER_FALLING | IRQF_ONESHOT,
					input_dev->name, ptr_zlg7290);
	if (ret) {
		dev_err(&client->dev, "irq %d busy?\n", client->irq);
		goto err_free_irq;
	}
	
	INIT_DELAYED_WORK(&ptr_zlg7290->work, zlg7290_kpad_work);
	ptr_zlg7290->delay = msecs_to_jiffies(30);
	device_init_wakeup(&client->dev, 1);

	ret = register_zlg7290_led(ptr_zlg7290);
	if (ret < 0)
		goto err_free_irq;

	return 0;

err_free_irq:
	free_irq(client->irq, ptr_zlg7290);
err_unreg_dev:
	input_unregister_device(input_dev);
	input_dev = NULL;
err_free_mem:
	input_free_device(input_dev);
	kfree(ptr_zlg7290);

	return ret;
}

static int zlg7290_remove(struct i2c_client *client) 
{
	struct zlg7290 *zlg7290 = i2c_get_clientdata(client);
	
	unregister_zlg7290_led(zlg7290);
	
	free_irq(client->irq, NULL);
	i2c_set_clientdata(client, NULL);
	
	input_unregister_device(zlg7290->input);
	input_free_device(zlg7290->input);
	
	kfree(zlg7290);
	return 0;
}

static const struct i2c_device_id zlg7290_id[] = {
	{ZLG7290_NAME, 0 },
	{ }
};
MODULE_DEVICE_TABLE(i2c, zlg7290_id);

#ifdef CONFIG_OF
static const struct of_device_id zlg7290_dt_ids[] = {
	{ .compatible = "myzr,zlg7290", },
	{ }
};
MODULE_DEVICE_TABLE(of, zlg7290_dt_ids);
#endif

static struct i2c_driver zlg7290_driver= {
	.driver	= {
		.name	= ZLG7290_NAME,
		.owner	= THIS_MODULE,
		.of_match_table = of_match_ptr(zlg7290_dt_ids),
	},
	.probe		= zlg7290_probe,
	.id_table	= zlg7290_id,
	.remove		= zlg7290_remove,
};

module_i2c_driver(zlg7290_driver);

MODULE_AUTHOR("buaa");
MODULE_DESCRIPTION("Keypad & Leds driver for ZLG7290");
MODULE_LICENSE("GPL");
                    </textarea>
                </div>
                <div class="col-md-4" style="height: 800px; padding-left: 5px;">
                    <textarea id="debug_text" class="form-control"></textarea>
                </div>
            </div>
        </asp:View>
        <asp:View ID="ModelView" runat="server">
            <div class="row">
                <div class="col-md-12" style="position: relative; margin-top: 20px; height: 855px; background-color: lightgray; border-style: solid; border-width: thin; border-color: darkgray">
                </div>
            </div>
        </asp:View>
        <asp:View ID="IntroView" runat="server">
            <h2>实验说明</h2>
            <hr />
            <div class="row">
                <div class="col-md-12" style="overflow-y:scroll; position: relative; height: 780px; background-color: whitesmoke; border-style: solid; border-width: thin; border-color: lightgray">
                    <h4>实验步骤1 内容简介</h4>
                    阅读实验原理，了解zlg7290的读写流程和I2C总线的使用方法<br />
                    根据实验步骤完成对zlg7290的读写程序设计和验证<br />
                    <br />
                    <h4>实验步骤2 实验目的</h4>
                    了解zlg7290的控制流程</h4>掌握使用I2C总线读写zlg7290的静态驱动程序设计方法<br />
                    <br />
                    <h4>实验环境</h4>
                    硬件：装有Linux操作系统的开发板<br />
                    软件：Ubuntu12.0，IDE，putty<br />
                    <br />
                    <h4>实验原理</h4>
                    【ZLG7290介绍】 ZLG7290 是广州周立功单片机发展有限公司自行设计的数码管显示驱动及键盘扫描管理芯片。能够直接驱动8位共阴极数码管（或64只独立的LED），同时还可以扫面管理多大64只按键。其中有8只按键可以作为功能键使用，就像电脑键盘上的Ctrl，Shift、Alt键一样。另外ZLG7290 内部还设有连击计数器，能够使某键按下后不松手而连续有效。该芯片为工业级芯片，抗干扰能力强，在工业测控中已有大量应用。该器件通过I2C总线接口进行操作，ZLG7290引脚图如图1。 
                    <br />
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Content/board.png" />
                    <br />
                    <h4>表 11.4说明了ZLG7290各引脚的功能。</h4>
                    <table style="width: 100%;" class="table">
                        <tr>
                            <td>引脚序号</td>
                            <td>引脚名</td>
                            <td>功能</td>
                        </tr>
                        <tr>
                            <td>1</td>
                            <td>SC/KR2</td>
                            <td>数码管c 段／键盘行信号2</td>
                        </tr>
                        <tr>
                            <td>2</td>
                            <td>SD/KR3</td>
                            <td>数码管d 段／键盘行信号3</td>
                        </tr>
                        <tr>
                            <td>3</td>
                            <td>DIG3/KC3</td>
                            <td>数码管位选信号3／键盘列信号3</td>
                        </tr>
                        <tr>
                            <td>4</td>
                            <td>DIG2/KC2</td>
                            <td>数码管位选信号2／键盘列信号2</td>
                        </tr>
                    </table>
                    <br />
                    数码管驱动zlg7290.c实现了设备文件操作控制，用户态可以调用zlg7290_hw_write()，zlg7290_hw_read()和zlg_led_ioctl()函数来对数码管进行读写操作，阅读ZLG7290数码管驱动的代码程序清单1.1，了解其实现的具体方法。
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
    <div class="row" style="position: relative; margin-top: 20px; height: 50px;">
        <div class="col-md-2">
            <asp:Button runat="server" OnClick="SwitchViewToIntro" Text="实验说明" CssClass="btn btn-default form-control" />
        </div>
        <div class="col-md-2">
            <asp:Button runat="server" OnClick="SwitchViewToCode" Text="代码编辑" CssClass="btn btn-default form-control" />
        </div>
        <div class="col-md-6"></div>
        <div class="col-md-2">
            <asp:Button runat="server" OnClick="SwitchViewToBoard" Text="板卡效果" CssClass="btn btn-default form-control" />
        </div>
    </div>

</asp:Content>
