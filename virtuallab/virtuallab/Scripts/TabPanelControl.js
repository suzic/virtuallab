
TabPanelControl = Ext.extend(Ext.TabPanel, {
    activeTab: 0,    
    deferredRender: false,
    enableTabScroll:true,
    defaults: { autoScroll: true },
    border:false,
    baseCls:"",
    onRender: function(ct, position) {
        Ext.TabPanel.superclass.onRender.call(this, ct, position);

        if (this.plain) {
            var pos = this.tabPosition == 'top' ? 'header' : 'footer';
            this[pos].addClass('x-tab-panel-' + pos + '-plain');
        }

        var st = this[this.stripTarget];

        this.stripWrap = st.createChild({ cls: 'x-tab-strip-wrap', cn: {
        tag: 'ul', cls: 'x-tab-strip legend-tab-strip'
        }
        });

        var beforeEl = (this.tabPosition == 'bottom' ? this.stripWrap : null);
        st.createChild({ cls: 'x-tab-strip-spacer' }, beforeEl);
        this.strip = new Ext.Element(this.stripWrap.dom.firstChild);

        // create an empty span with class x-tab-strip-text to force the height of the header element when there's no tabs.
        this.edge = this.strip.createChild({ tag: 'li', cls: 'x-tab-edge', cn: [{ tag: 'span', cls: 'x-tab-strip-text', cn: '&#160;'}] });
        this.strip.createChild({ cls: 'x-clear' });

        this.body.addClass('x-tab-panel-body-' + this.tabPosition);
        if (!this.itemTpl) {
            var tt = new Ext.Template(
                 '<li class="{cls}" id="{id}"><a class="x-tab-strip-close"></a>',
                 '<a class="x-tab-right" href="#"><em class="x-tab-left">',
                 '<span class="x-tab-strip-inner" style="padding-left:6px "><span class="x-tab-strip-text {iconCls}" style="color:black">{text}</span></span>',
                 '</em></a></li>'
            );
            tt.disableFormats = true;
            tt.compile();
            Ext.TabPanel.prototype.itemTpl = tt;
        }

        this.items.each(this.initTab, this);
    }

})
