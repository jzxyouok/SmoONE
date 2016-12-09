using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Smobiler.Core;
using Smobiler.Core.Controls;
using SmoONE.Domain;
using SmoONE.DTOs;
using SmoONE.UI.UserInfo;

namespace SmoONE.UI.Department
{
    // ******************************************************************
    // �ļ��汾�� SmoONE 1.0
    // Copyright  (c)  2016-2017 Smobiler 
    // ����ʱ�䣺 2016/11
    // ��Ҫ���ݣ�  �����б�����
    // ******************************************************************
    partial class frmDepartment : Smobiler.Core.MobileForm
    {
        #region "definition"
        private DepartmentMode Mode; //�ͻ�չʾģʽ
        AutofacConfig AutofacConfig = new AutofacConfig();//����������
        #endregion
        /// <summary>
        /// gridDepData����¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridDepData_CellClick(object sender, GridViewCellEventArgs e)
        {
            string D_ID = e.Cell.Items["lblId"].Value.ToString();
            //��ת��������ϸ����
            frmDepartmentDetail frm = new frmDepartmentDetail();
            frm.D_ID = D_ID;
            Redirect(frm, (MobileForm form, object args) =>
            {
                if (frm.ShowResult == ShowResult.Yes)
                {
                    Mode = DepartmentMode.�б�;
                    Bind();
                }
            });
        }
        /// <summary>
        /// ��ʼ���¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmDepartment_Load(object sender, EventArgs e)
        {
            Mode = DepartmentMode.�б�;
            Bind();
        }
        /// <summary>
        /// ��ʼ������
        /// </summary>
        private void Bind()
        {
            try
            {
                //��ȡ���в�������
                List<DepartmentDto> listDep = AutofacConfig.departmentService.GetAllDepartment();
                switch (Mode)
                {
                    case DepartmentMode.�б�:
                        gridDepData.Rows.Clear();//��ղ����б�����
                        break;
                    case DepartmentMode.�㼶:
                        treeDepData.Nodes.Clear();//��ղ��Ų㼶����
                        break;
                }

                if (listDep.Count > 0)
                {
                    lblInfor.Visible = false ;
                    foreach (DepartmentDto dep in listDep)
                    {
                        if (string.IsNullOrEmpty(dep.Dep_Icon) == true)
                        {
                            dep.Dep_Icon = "bumenicon";
                        }
                      
                    }
                    switch (Mode)
                    {
                        case  DepartmentMode.�б�:
                            gridDepData.Visible = true;
                            treeDepData.Visible = false;
                            gridDepData.DataSource = listDep;
                            gridDepData.DataBind();
                            break;
                        case DepartmentMode.�㼶:
                            gridDepData.Visible = false;
                            treeDepData.Visible = true;
                            foreach (DepartmentDto dep in listDep)
                            {
                                TreeViewNode node = new TreeViewNode(dep.Dep_Name, null, dep.Dep_Icon, (int)TreeMode.dep+","+dep.Dep_ID);
                                node.TextColor = System.Drawing.Color.FromArgb(45,45,45);
                                List<UserDto> listDepUser = AutofacConfig.userService.GetUserByDepID(dep.Dep_ID);
                                if (listDepUser.Count > 0)
                                {
                                    foreach (UserDto user in listDepUser)
                                    {
                                        string Name="";
                                        if (dep .Dep_Leader .Equals (user.U_ID))
                                        {
                                            Name=user.U_Name+"  ������";
                                        }
                                        else 
                                        {
                                            Name=user.U_Name;
                                        }
                                        string portrait="";
                                        if (string.IsNullOrEmpty(user.U_Portrait) == true)
                                        {
                                            switch (user.U_Sex)
                                            {
                                                case (int)Sex.��:
                                                    portrait = "boy";
                                                    break;
                                                case (int)Sex.Ů:
                                                    portrait = "girl";
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            portrait = user.U_Portrait;
                                        }
                                        TreeViewNode node1 = new TreeViewNode(Name, null, portrait, (int)TreeMode.user+","+user.U_ID);
                                        node1.TextColor = System.Drawing.Color.FromArgb(145,145,145);
                                       
                                        node.Nodes.Add(node1);
                                    }
                                  
                                }
                                treeDepData.Nodes.Add(node);
                            }
                            break;
                    }
                    
                }
                else
                {
                    lblInfor.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }
        /// <summary>
        /// �ֻ��Դ����˰�ť�¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmDepartment_KeyDown(object sender, KeyDownEventArgs e)
        {
            if (e.KeyCode == KeyCode.Back)
            {
                Close();         //�رյ�ǰҳ��
            }
        }
        /// <summary>
        /// ������ͼƬ��ť����¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmDepartment_TitleImageClick(object sender, EventArgs e)
        {
            Close();
        }
        /// <summary>
        /// ��ת���������Ž���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCreate_Click(object sender, EventArgs e)
        {
            frmDepartmentCreate frm = new frmDepartmentCreate();
            Redirect(frm, (MobileForm form, object args) =>
                {
                    if (frm.ShowResult == ShowResult.Yes)
                    {
                        Bind();
                    }
                });
        }

        /// <summary>
        /// treeDepData����¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeDepData_NodeSelected(object sender, EventArgs e)
        {
            string ID = treeDepData.SelectedNode.Value;
            switch (Convert .ToInt32 (ID.Split(',')[0]))
            {
                case (int)TreeMode.dep:
                    frmDepartmentDetail frm = new frmDepartmentDetail();
                    frm.D_ID = ID.Split(',')[1];
                    Redirect(frm );
                    break;
                case (int)TreeMode.user:
                    frmUserDetail frmUserDetail = new frmUserDetail();
                    frmUserDetail.U_ID = ID.Split(',')[1];
                    Redirect(frmUserDetail);
                    break;
            }
           
        }
        /// <summary>
        /// ������ʾģʽ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmDepartment_FooterBarLayoutItemClick(object sender, MobileFormLayoutItemEventArgs e)
        {
            switch (Mode)
            {
                case DepartmentMode.�б�:
                    Mode = DepartmentMode.�㼶;
                    break;
                case DepartmentMode.�㼶:
                    Mode = DepartmentMode.�б�;
                    break;
            }
            Bind();
        }
    }
}