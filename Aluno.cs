using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Exemplo.Model
{
    public class Aluno : Base
    {
        [Key]
        public int cod_aluno { get; set; }

        [Display(Name = "Nome do Aluno")]
        public string nom_aluno { get; set; }

        [Display(Name = "CPF do Aluno")]
        public string num_cpf { get; set; }

        [Display(Name = "UF")]
        public UF uf { get; set; }

        public List<Aluno> listar()
        {
            List<Aluno> retorno = new List<Aluno>();
            Aluno objeto = new Aluno();

            try
            {
                cnn.Open();
                cmd.Connection = cnn;

                sql.Append(" SELECT ");
                sql.Append(" * ");
                sql.Append(" FROM ");
                sql.Append(" ALUNO ");
                sql.Append(" WHERE 1=1 ");
                if (this.cod_aluno > 0)
                {
                    sql.Append(" AND COD_ALUNO = @COD_ALUNO ");
                    cmd.Parameters.AddWithValue("@COD_ALUNO", this.cod_aluno);
                }
                if (this.nom_aluno != null && this.nom_aluno != "")
                {
                    sql.Append(" AND NOM_ALUNO LIKE @NOM_ALUNO ");
                    cmd.Parameters.AddWithValue("@NOM_ALUNO", "%" + this.nom_aluno + "%");
                }
                if (this.num_cpf != null && this.num_cpf != "")
                {
                    sql.Append(" AND NUM_CPF LIKE @NUM_CPF ");
                    cmd.Parameters.AddWithValue("@NUM_CPF", "%" + this.num_cpf + "%");
                }
                sql.Append(" order by NOM_ALUNO ");
                cmd.CommandText = sql.ToString();
                dt.Load(cmd.ExecuteReader());
                cmd.Parameters.Clear();
                sql.Remove(0, sql.Length);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        objeto = new Aluno();
                        setReflection(dt.Rows[i], objeto);
                        if (getValue(dt.Rows[i]["COD_UF"]) != null )
                        {
                            objeto.uf = new UF() { cod_uf = getValue(dt.Rows[i]["COD_UF"]) }.listar().First();
                        }
                        retorno.Add(objeto);
                    }
                }
                return retorno;
            }
            catch (Exception ex)
            {
                string erro = ex.Message;
                throw new Exception("Atenção ocorreu um erro na execução dessa rotina.");
            }
        }

        public void Gravar()
        {
            try
            {
                using (cnn)
                {
                    cnn.Open();
                    cmd.Connection = cnn;
                    sql.Append(" INSERT INTO ALUNO ");
                    sql.Append(" ( ");
                    sql.Append(" NOM_ALUNO ");
                    sql.Append(" ,NUM_CPF ");
                    sql.Append(" ,COD_UF ");
                    sql.Append(" ) ");
                    sql.Append(" VALUES ");
                    sql.Append(" ( ");
                    sql.Append(" @NOM_ALUNO ");
                    sql.Append(" ,@NUM_CPF ");
                    sql.Append(" ,@COD_UF ");
                    sql.Append(" ) ");

                    cmd.Parameters.AddWithValue("@NOM_ALUNO", this.nom_aluno);
                    cmd.Parameters.AddWithValue("@NUM_CPF", this.num_cpf);

                    if (this.uf == null)
                        cmd.Parameters.AddWithValue("@COD_UF", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@COD_UF", this.uf.cod_uf);

                    cmd.CommandText = sql.ToString();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                string erro = ex.Message;
                throw new Exception("Atenção ocorreu um erro na execução dessa rotina.");
            }
        }

        public void Alterar()
        {
            try
            {
                using (cnn)
                {
                    cnn.Open();
                    cmd.Connection = cnn;
                    sql.Append(" UPDATE ");
                    sql.Append(" FUNCIONARIO ");
                    sql.Append(" SET ");
                    sql.Append(" NOM_ALUNO = @NOM_ALUNO ");
                    sql.Append(" ,NUM_CPF = @NUM_CPF ");
                    sql.Append(" WHERE ");
                    sql.Append(" COD_ALUNO = @COD_ALUNO ");
                    cmd.Parameters.AddWithValue("@COD_ALUNO", isNULL(this.cod_aluno));
                    cmd.Parameters.AddWithValue("@NOM_ALUNO", isNULL(this.nom_aluno));
                    cmd.Parameters.AddWithValue("@NUM_CPF", isNULL(this.num_cpf));
                    cmd.CommandText = sql.ToString();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                string erro = ex.Message;
                throw new Exception("Atenção ocorreu um erro na execução dessa rotina.");
            }
        }

        public void Excluir()
        {
            using (cnn)
            {
                try
                {
                    cnn.Open();
                    cmd.Connection = cnn;
                    sql.Append(" DELETE FROM ALUNO ");
                    sql.Append(" where COD_ALUNO = @COD_ALUNO");
                    cmd.Parameters.AddWithValue("@COD_ALUNO", this.cod_aluno);
                    cmd.CommandText = sql.ToString();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string erro = ex.Message;
                    throw new Exception("Atenção ocorreu um erro na execução dessa rotina.");
                }
            }
        }
    }
}
