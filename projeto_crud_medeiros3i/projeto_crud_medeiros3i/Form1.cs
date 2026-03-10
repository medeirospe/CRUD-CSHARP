using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace projeto_crud_medeiros3i
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public NpgsqlConnection Conectar;
        public NpgsqlCommand ComandoSQL;
        public NpgsqlDataReader LerRegistro;
        public string strCon = "Server=localhost;Port=5432;Username=postgres;password=1804pedro;Database=bancox_medeiros";
        public  string strSQL =  "";

        public void LimparObjetos()
        {
            //Limpar Objetos do FormulÃ¡rio
            mkt_cpf.Clear();
            txt_nome.Clear();
            txt_telefone.Clear();
            txt_email.Clear();
            mkt_dn.Clear();
            return;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            btn_alterar.Enabled = false;
            btn_excluir.Enabled = false;
            btn_cancelar.Visible = false;
        }

        private void btn_inserir_Click(object sender, EventArgs e)
        {
            try
            {
                if (mkt_cpf.Text.Trim().Length == 14)
                {
                    Conectar = new NpgsqlConnection(strCon);
                    Conectar.Open();

                    strSQL = "SELECT * FROM clientes WHERE cpf = '" + mkt_cpf + "'";
                    MessageBox.Show(strSQL);

                    ComandoSQL = new NpgsqlCommand(strSQL, Conectar);

                    LerRegistro = ComandoSQL.ExecuteReader();

                    if (LerRegistro.Read())
                    {
                        MessageBox.Show("Cliente Já Existente!"," Sistema Informa!");
                    }
                    else
                    {
                        LerRegistro.Close();
                        ComandoSQL.Dispose();
                        ComandoSQL.Transaction = null;

                        strSQL = "INSERT INTO clientes (cpf, nome, telefone, email, dn) VALUES ("
                            + "'" + mkt_cpf.Text + "', " +
                            "'" + txt_nome.Text + "', " +
                            "'" + txt_telefone.Text + "', " +
                            "'" + txt_email.Text + "', " +
                            "TO_DATE ('" + mkt_dn.Text + "', 'DD/MM/YYYY'));";

                        MessageBox.Show(strSQL);

                        ComandoSQL = new NpgsqlCommand(strSQL, Conectar);
                        ComandoSQL.ExecuteNonQuery();
                        MessageBox.Show("Registro Inserido com Sucesso!"," Sistema Informa");

                        LimparObjetos();
                    }

                    Conectar.Close();

                }
                else
                {
                    MessageBox.Show("Preencher corretamente o campo CPF!"," Sistema Informa");
                }
                mkt_cpf.Focus();
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message);
            }
        }

        private void btn_consultar_Click(object sender, EventArgs e)
        {
            try
            {
                if (mkt_cpf.Text.Trim().Length == 14)
                {
                    Conectar = new NpgsqlConnection(strCon);
                    Conectar.Open();

                    strSQL = "SELECT * FROM clientes WHERE cpf = '" + mkt_cpf + "'";
                    MessageBox.Show(strSQL);

                    ComandoSQL = new NpgsqlCommand(strSQL, Conectar);
                    LerRegistro = ComandoSQL.ExecuteReader();

                    if (LerRegistro.Read())
                    {
                        mkt_cpf.Text = LerRegistro.GetString(0);
                        txt_nome.Text = LerRegistro.GetString(1);
                        txt_telefone.Text = LerRegistro.GetString(2);
                        txt_email.Text = LerRegistro.GetString(3);
                        mkt_dn.Text = LerRegistro.GetDateTime(4).ToString("dd/MM/yyyy");

                        if (DialogResult.No == MessageBox.Show("Deseja Editar Registro?", "Sistema Informa", MessageBoxButtons.YesNo, 
                            MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
                        {
                            LimparObjetos();
                            btn_alterar.Enabled = false;
                            btn_excluir.Enabled = false;
                            mkt_cpf.Enabled = true;
                        }
                        else
                        {
                            btn_alterar.Enabled = true;
                            btn_excluir.Enabled = true;
                            btn_consultar.Enabled = false;
                            btn_inserir.Enabled = false;
                            mkt_cpf.Enabled = false;
                            btn_cancelar.Visible = true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("CPF: " + mkt_cpf.Text + "Não Encontrado!", "Sistema Informa");
                        mkt_cpf.Focus();
                    }
                    LerRegistro.Close();
                    ComandoSQL.Dispose();
                    ComandoSQL.Transaction = null;
                    Conectar.Close();
                }
                else
                {
                    MessageBox.Show("Preencha Corretamente o campo CPF!", "Sistema Informa");
                }
                mkt_cpf.Focus();
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message);
            }
        }
    }
}
